using Mapster;
using SmartBus.Application.Dtos.LocationDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class LocationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Location, LocationDto>();  
        }
    }
}
