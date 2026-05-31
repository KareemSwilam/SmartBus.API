using Mapster;
using SmartBus.Application.Dtos.BookingDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class BookingMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Booking, UserBookingDto>()
                .Map(dest => dest.BookingId, src =>src.Id)
                .Map(dest => dest.StartLocation, src => src.Trip.StartLocation.Name)
                .Map(dest => dest.EndLocation, src => src.Trip.EndLocation.Name)
                .Map(dest => dest.DepartureTime, src => src.Trip.DepartureTime)
                .Map(dest => dest.ArrivalTime, src => src.Trip.ArrivalTime)
                .Map(dest => dest.CompanyName, src => src.Trip.Company.Name)
                .Map(dest => dest.BusNumber, src => src.Trip.Bus.PlateNumber)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.SeatNumber, src => src.SeatNumber);
        }
    }
}
