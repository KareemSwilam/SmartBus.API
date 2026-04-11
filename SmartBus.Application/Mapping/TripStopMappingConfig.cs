using Mapster;
using SmartBus.Application.Dtos.TripStopsDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class TripStopMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AddingStopDto, TripStop>();
            config.NewConfig<TripStop,StopDto>()
                  .Map(dest => dest.Location , scr => scr.Location.Name);   
        }
    }
}
