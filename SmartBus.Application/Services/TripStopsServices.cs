using MapsterMapper;
using Microsoft.VisualBasic;
using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Application.Dtos.TripStopsDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.IRepository;
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

        public async Task<CustomResult<TripWithStops>> AddingStop(AddingStopDto dto)
        {
            var tripExist = await _unitOfWork.TripRepository.Get(t => t.Id == dto.TripId);
            if(tripExist == null) 
                return CustomResult<TripWithStops>.Failure(CustomError.NotFound("Trip not found"));
            var LoctionExist = await _unitOfWork.LocationRepository.Get(l => l.Id == dto.LocationId);    
            if(LoctionExist == null)
                return CustomResult<TripWithStops>.Failure(CustomError.NotFound("Location not found"));    
            var stopOfSameTrip = await _unitOfWork.TripStopRepository.GetAll(ts => ts.TripId == dto.TripId);
            if(stopOfSameTrip.Any(ts => ts.StopOrder == dto.StopOrder))
                return CustomResult<TripWithStops>.Failure(CustomError.InvalidInput("Stop order already exist for this trip"));
            var previousStop = stopOfSameTrip.FirstOrDefault(ts => ts.StopOrder == dto.StopOrder - 1);
            if (previousStop != null && previousStop.DepatureTime >= dto.ArrivalTime)
                return CustomResult<TripWithStops>.Failure(CustomError.InvalidInput("Arrival time must be after the departure time of the previous stop"));
            if (dto.StopOrder > 1 && previousStop == null)
                return CustomResult<TripWithStops>.Failure(CustomError.InvalidInput("Previous stop must exist"));
            if (previousStop == null && dto.ArrivalTime > dto.DepatureTime)
                return CustomResult<TripWithStops>.Failure(CustomError.InvalidInput("Arrival time must be before the departure time of the trip"));
            if (dto.ArrivalTime >= dto.DepatureTime)
                return CustomResult<TripWithStops>.Failure(CustomError.InvalidInput("Arrival time must be before departure time"));
            var nextStop = stopOfSameTrip.FirstOrDefault(ts => ts.StopOrder == dto.StopOrder + 1 );
            if (nextStop != null && nextStop.ArrivalTime <= tripExist.DepartureTime)
                return CustomResult<TripWithStops>.Failure(CustomError.InvalidInput("Departure time must be before the arrival time of the next stop"));
            var tripStop = _mapper.Map<Domain.Models.TripStop>(dto);
            await _unitOfWork.TripStopRepository.Add(tripStop);
            var complete = await _unitOfWork.SaveAsync() > 0;
            if (!complete)
                return CustomResult<TripWithStops>.Failure(CustomError.ServerError("Failed to add trip stop"));
            var trip = await _unitOfWork.TripRepository.GetTripWithStops(tripExist.Id);
            var result = _mapper.Map<TripWithStops>(trip);
            return CustomResult<TripWithStops>.Success(result);
        }
        public async Task<CustomResult<TripWithStops>> TripWithStops(Guid id)
        {
            var trip = await _unitOfWork.TripRepository.GetTripWithStops(id);
            if (trip == null)
                CustomResult<TripWithStops>.Failure(CustomError.NotFound("Trip Not Exist"));
            var result = _mapper.Map<TripWithStops>(trip!);
            return CustomResult<TripWithStops>.Success(result);
        }
    }
}
