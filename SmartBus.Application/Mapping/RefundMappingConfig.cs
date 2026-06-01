using Mapster;
using SmartBus.Application.Dtos.RefundRequestDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class RefundMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RefundRequest, RefundRequestDto>()
                .Map(dest => dest.Status, src => src.Status.ToString());
        }
    }
}
