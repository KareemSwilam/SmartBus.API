using Mapster;
using SmartBus.Application.Dtos.SeatDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class SeatMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Seat, SeatDto>();
        }
    }
}
