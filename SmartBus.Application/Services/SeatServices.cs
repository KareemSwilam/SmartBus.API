using MapsterMapper;
using SmartBus.Application.Dtos.SeatDtos;
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
    public class SeatServices : ISeatServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public SeatServices(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }   
        public async Task<CustomResult<AvailableSeatsDto>> AvailableSeatsInTrip(Guid tripId, int? startLocationId , int? endLocationId )
        {
            var TripExist = await _unit.TripRepository.GetTripWithStops(tripId);
            if (TripExist == null)
                return CustomResult<AvailableSeatsDto>.Failure(CustomError.NotFound("Trip You try Booking in Not Exist"));
            var LocationId = TripExist!.TripStops.Select(s => s.LocationId).ToList();
            var FromStop = new TripStop();
            if (startLocationId is null)
                FromStop = TripExist.TripStops.FirstOrDefault(ts => ts.IsStartStop);
            else
                FromStop = TripExist.TripStops.FirstOrDefault(ts => ts.LocationId == startLocationId);
            if(FromStop == null)
                return CustomResult<AvailableSeatsDto>.Failure(CustomError.InvalidInput("Invalid start location"));
            var ToStop = new TripStop();
            if (endLocationId is null)
                ToStop = TripExist.TripStops.FirstOrDefault(ts => ts.IsEndStop);
            else
                ToStop = TripExist.TripStops.FirstOrDefault(ts => ts.LocationId == endLocationId);
            if(ToStop == null)
                return CustomResult<AvailableSeatsDto>.Failure(CustomError.InvalidInput("Invalid end location"));
            if (!LocationId.Contains(FromStop.LocationId) || !LocationId.Contains(ToStop.LocationId))
                return CustomResult<AvailableSeatsDto>.Failure(CustomError.InvalidInput("Invalid start or end location"));
            
            var FromStopOrder = FromStop.StopOrder;
            var ToStopOrder = ToStop.StopOrder;
            if (FromStopOrder >= ToStopOrder)
                return CustomResult<AvailableSeatsDto>.Failure(CustomError.InvalidInput("Invalid trip direction"));
            
            var ResevedSeatId = await _unit.ReservedSeatRepository.GetReservedSeatsIdInTrip(tripId, FromStopOrder, ToStopOrder);
            var FreeSeat = await _unit.SeatRepository.GetAll(s => !ResevedSeatId.Contains(s.Id) && s.BusId == TripExist.BusId);
            var AvailableSeats =new AvailableSeatsDto
            {
                Count = FreeSeat.Count(),
                Seats = _mapper.Map<List<SeatDto>>(FreeSeat)
            };
            return CustomResult<AvailableSeatsDto>.Success(AvailableSeats);
        }

        public async Task<CustomResult<ReservedSeatDto>> ReservedSeatsInTrip(Guid tripId, int? startLocationId = 0, int? endLocationId = 0)
        {
            var TripExist = await _unit.TripRepository.GetTripWithStops(tripId);
            if (TripExist == null)
                return CustomResult<ReservedSeatDto>.Failure(CustomError.NotFound("Trip You try Booking in Not Exist"));
            var LocationId = TripExist!.TripStops.Select(s => s.LocationId).ToList();
            var FromStop = new TripStop();
            if (startLocationId is null)
                FromStop = TripExist.TripStops.FirstOrDefault(ts => ts.IsStartStop);
            else
                FromStop = TripExist.TripStops.FirstOrDefault(ts => ts.LocationId == startLocationId);
            if (FromStop == null)
                return CustomResult<ReservedSeatDto>.Failure(CustomError.InvalidInput("Invalid start location"));
            var ToStop = new TripStop();
            if (endLocationId is null)
                ToStop = TripExist.TripStops.FirstOrDefault(ts => ts.IsEndStop);
            else
                ToStop = TripExist.TripStops.FirstOrDefault(ts => ts.LocationId == endLocationId);
            if (ToStop == null)
                return CustomResult<ReservedSeatDto>.Failure(CustomError.InvalidInput("Invalid end location"));
            if (!LocationId.Contains(FromStop.LocationId) || !LocationId.Contains(ToStop.LocationId))
                return CustomResult<ReservedSeatDto>.Failure(CustomError.InvalidInput("Invalid start or end location"));

            var FromStopOrder = FromStop.StopOrder;
            var ToStopOrder = ToStop.StopOrder;
            if (FromStopOrder >= ToStopOrder)
                return CustomResult<ReservedSeatDto>.Failure(CustomError.InvalidInput("Invalid trip direction"));
            var resevedSeats = await _unit.ReservedSeatRepository.GetReservedSeatsIdInTripWithDetails(tripId, FromStopOrder, ToStopOrder);
            var seats = resevedSeats.GroupBy(r => r.SeatId).Select(g => new SeatWithReservedInfoDto
            {
                Id = g.Key,
                SeatNumber = g.Select(g => g.Seat.SeatNumber).First(),
                Reserveds = g.Select(g => new ReservedInfoDto
                {
                    StartStopOrder = g.StartStopOrder,
                    EndStopOrder = g.EndStopOrder,  
                }).ToList()
            }).ToList();
            var result = new ReservedSeatDto {
                TripId = tripId,
                PlateNumber = resevedSeats.Select(g => g.Bus.PlateNumber).First(),
                Seats = seats,
            };
            return CustomResult<ReservedSeatDto>.Success(result);
        }

    }
}
