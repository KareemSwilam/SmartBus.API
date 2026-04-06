using Mapster;
using SmartBus.Application.Dtos.BusDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class BusMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateBusDto, Bus>()
                .Map(dest => dest.CompanyId, src => src.CompanyId)
                .Map(dest => dest.Type, src => src.Type)
                .Map(dest => dest.PlateNumber, src => src.PlateNumber)
                .Map(dest => dest.BusNumber, src => src.BusNumber)
                .Map(dest => dest.NumberSeats, src => src.SeatCount);
            config.NewConfig<Bus, BusDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => src.Type.ToString())
                .Map(dest => dest.PlateNumber, src => src.PlateNumber)
                .Map(dest => dest.BusNumber, src => src.BusNumber)
                .Map(dest => dest.NumberSeats, src => src.NumberSeats);

        }
    }
}
