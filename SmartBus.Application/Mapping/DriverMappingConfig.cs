using Mapster;
using SmartBus.Application.Dtos.DriverDtos;
using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class DriverMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Driver, DriverDto>();
            config.NewConfig<CreateDriverDto, Driver>();
            config.NewConfig<UpdateDriverDto, Driver>();
            config.NewConfig<Driver, DriverWithTrips>();
        }
    }       
}
