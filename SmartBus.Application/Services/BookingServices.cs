using SmartBus.Application.Dtos.BookingDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.Enums;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork _unit;
        public BookingServices(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public async Task<CustomResult> BookingSeat(BookingRequestDto requestDto, string userId)
        {
            var TripExist = await _unit.TripRepository.GetTripWithStops(requestDto.TripId);
            if (TripExist == null)
               return CustomResult.Failure(CustomError.NotFound("Trip You try Booking in Not Exist"));
            var LocationId = TripExist!.TripStops.Select(s => s.LocationId).ToList();
            if (!LocationId.Contains(requestDto.StartLocationId)|| !LocationId.Contains(requestDto.EndLocationId))
                return CustomResult.Failure(CustomError.InvalidInput("Invalid start or end location"));
            if(!(TripExist.BusId == requestDto.BusId))
                return CustomResult.Failure(CustomError.InvalidInput("This bus Not in This trip"));
            var bus = await _unit.BusRepository.GetBusWithSeat(requestDto.BusId); 
            var SeatsIds = bus.Seats.Select(s => s.Id).ToList();    
            if(!SeatsIds.Contains(requestDto.SeatId))
                return CustomResult.Failure(CustomError.InvalidInput("InValid Seat Number"));
            var FromStopOrder = TripExist.TripStops.First(ts => ts.LocationId ==  requestDto.StartLocationId).StopOrder;
            var ToStopOrder = TripExist.TripStops.First(ts => ts.LocationId == requestDto.EndLocationId).StopOrder;
            var Price = await _unit.StopSegmentRepository.GetPriceForSegment(requestDto.TripId, FromStopOrder, ToStopOrder);
            var booking = new Booking 
            { 
                TripId = requestDto.TripId,
                StartLocationId = requestDto.StartLocationId,
                EndLocationId = requestDto.EndLocationId,
                SeatNumber = bus.Seats.First(s => s.Id == requestDto.SeatId).SeatNumber,
                UserId = userId,
                Price = Price,
                Status = BookingStatus.pending

            };
            var ReservedSeat = new ReservedSeat
            {
                TripId = requestDto.TripId,
                StartStopOrder = FromStopOrder,
                EndStopOrder = ToStopOrder,
                SeatId = requestDto.SeatId,
                BusId = requestDto.BusId,
            };
            await _unit.BookingRepository.Add(booking);
            await _unit.ReservedSeatRepository.Add(ReservedSeat);   
            await _unit.SaveAsync();    
            return CustomResult.Success();


        }
    }
}
