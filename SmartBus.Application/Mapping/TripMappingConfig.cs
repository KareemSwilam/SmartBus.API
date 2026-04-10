using Mapster;
using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class TripMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Trip, TripDto>();
            config.NewConfig<CreateTripDto, Trip>();
            config.NewConfig<Trip, TripDetailsDto>()
                .Map(dest => dest.BusPlateNumber, src => src.Bus.PlateNumber)
                .Map(dest => dest.DriverName, src => src.Dervier.FullName)
                .Map(dest => dest.CompanyName, src => src.Company.Name)
                .Map(dest => dest.StartLocation, src => src.StartLocation)
                .Map(dest => dest.EndLocation, src => src.EndLocation)
                .Map(dest => dest.DepartureTime, src => src.DepartureTime)
                .Map(dest => dest.ArrivalTime, src => src.ArrivalTime)
                .Map(dest=> dest.Price, src => src.Price);      


        }
    }
}
