using MapsterMapper;
using Microsoft.VisualBasic;
using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Application.Dtos.TripStopsDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Services
{
    public class TripStopsServices:ITripStopsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TripStopsServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomResult<TripWithStops>> AddingStops(AddingStopRequestDto dto)
        {
            var tripExist = await _unitOfWork.TripRepository
                .Get(t => t.Id == dto.TripId);

            if (tripExist == null)
                return CustomResult<TripWithStops>
                    .Failure(CustomError.NotFound("Trip not found"));

            var existingStops = (await _unitOfWork.TripStopRepository
                .GetAll(ts => ts.TripId == dto.TripId))
                .OrderBy(ts => ts.StopOrder)
                .ToList();

            if (!existingStops.Any())
                return CustomResult<TripWithStops>
                    .Failure(CustomError.NotFound("Trip Stops Not Found"));

            var orderedStops = dto.Stops
                .OrderBy(s => s.StopOrder)
                .ToList();

            if (!orderedStops.Any())
                return CustomResult<TripWithStops>
                    .Failure(CustomError.InvalidInput("No Stops Added"));

            

            var duplicateOrders = orderedStops
                .GroupBy(s => s.StopOrder)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateOrders.Any())
                return CustomResult<TripWithStops>
                    .Failure(CustomError.InvalidInput(
                        $"Duplicate Stop Orders: {string.Join(",", duplicateOrders)}"));


            for (int i = 0; i < orderedStops.Count; i++)
            {
                var expectedOrder = i + 2;

                if (orderedStops[i].StopOrder != expectedOrder)
                {
                    return CustomResult<TripWithStops>
                        .Failure(CustomError.InvalidInput(
                            $"Invalid Stop Order Sequence. Expected Order {expectedOrder}"));
                }
            }

            

            var locationIds = orderedStops
                .Select(s => s.LocationId)
                .Distinct()
                .ToList();

            var locations = await _unitOfWork.LocationRepository
                .GetAll(l => locationIds.Contains(l.Id));

            if (locations.Count() != locationIds.Count)
                return CustomResult<TripWithStops>
                    .Failure(CustomError.InvalidInput("One Or More Locations Not Exist"));

            var duplicateLocations = orderedStops
                .GroupBy(s => s.LocationId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateLocations.Any())
            {
                return CustomResult<TripWithStops>
                    .Failure(CustomError.InvalidInput(
                        "Duplicate Locations Are Not Allowed"));
            }


            var startStop = existingStops
                .FirstOrDefault(ts => ts.IsStartStop);

            var endStop = existingStops
                .FirstOrDefault(ts => ts.IsEndStop);

            if (startStop == null || endStop == null)
            {
                return CustomResult<TripWithStops>
                    .Failure(CustomError.ServerError(
                        "Trip Structure Invalid"));
            }

            

            for (int i = 0; i < orderedStops.Count; i++)
            {
                var currentStop = orderedStops[i];

                
                if (currentStop.ArrivalTime >= currentStop.DepatureTime)
                {
                    return CustomResult<TripWithStops>
                        .Failure(CustomError.InvalidInput(
                            $"Arrival Time Must Be Before Departure Time At Stop Order {currentStop.StopOrder}"));
                }

               
                if (currentStop.StopOrder == 2)
                {
                    if (startStop.DepatureTime >= currentStop.ArrivalTime)
                    {
                        return CustomResult<TripWithStops>
                            .Failure(CustomError.InvalidInput(
                                $"Invalid Arrival Time At Stop Order {currentStop.StopOrder}"));
                    }
                }

                

                if (currentStop.StopOrder > 2)
                {
                    var previousStop = orderedStops[i - 1];

                    if (previousStop.DepatureTime >= currentStop.ArrivalTime)
                    {
                        return CustomResult<TripWithStops>
                            .Failure(CustomError.InvalidInput(
                                $"Invalid Arrival Time At Stop Order {currentStop.StopOrder}"));
                    }
                }

               

                if (currentStop.DepatureTime >= endStop.ArrivalTime)
                {
                    return CustomResult<TripWithStops>
                        .Failure(CustomError.InvalidInput(
                            $"Departure Time Of Stop Order {currentStop.StopOrder} Must Be Before Trip Arrival Time"));
                }
            }

           

            var firstInsertedOrder = orderedStops.Min(s => s.StopOrder);

            var shiftCount = orderedStops.Count;

            var stopsToShift = existingStops
                .Where(ts => ts.StopOrder >= firstInsertedOrder)
                .ToList();

            foreach (var stop in stopsToShift)
            {
                stop.StopOrder += shiftCount;

                _unitOfWork.TripStopRepository.Update(stop);
            }

            
            var newStops = orderedStops
                .Select(stop => new TripStop
                {
                    TripId = dto.TripId,
                    LocationId = stop.LocationId,
                    StopOrder = stop.StopOrder,
                    ArrivalTime = stop.ArrivalTime,
                    DepatureTime = stop.DepatureTime,
                    IsStartStop = false,
                    IsEndStop = false
                })
                .ToList();
            await AddSegments(dto);

            _unitOfWork.TripStopRepository.AddRange(newStops);
            
            var complete = await _unitOfWork.SaveAsync();

            if (complete < 1)
            {
                return CustomResult<TripWithStops>
                    .Failure(CustomError.ServerError(
                        "Failed To Add Stops"));
            }

            return await TripWithStops(dto.TripId);
        }
        public async Task<CustomResult<TripWithStops>> TripWithStops(Guid id)
        {
            var trip = await _unitOfWork.TripRepository.GetTripWithStops(id);
            if (trip == null)
                return CustomResult<TripWithStops>.Failure(CustomError.NotFound("Trip Not Exist"));
            var result = _mapper.Map<TripWithStops>(trip!);
            result.Stops = result.Stops.OrderBy(s => s.StopOrder).ToList();
            return CustomResult<TripWithStops>.Success(result);
        }
        private async Task<bool> AddSegments(AddingStopRequestDto dto)
        {
            var segments = dto.Segments.Select(s => new StopSegment
            {
                TripId = dto.TripId,
                FromStopOrder = s.FromStopOrder,
                ToStopOrder = s.ToStopOrder,
                Price = s.Price                
            }).ToList();
            _unitOfWork.StopSegmentRepository.AddRange(segments);
            var complete = await _unitOfWork.SaveAsync();
            return complete > 0;
        }
    }
}
