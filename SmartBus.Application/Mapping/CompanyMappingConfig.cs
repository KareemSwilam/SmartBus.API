using Mapster;
using SmartBus.Application.Dtos.CompanyDtos;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Mapping
{
    public class CompanyMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Company, CompanyDto>();
            config.NewConfig<CreateCompanyDto, Company>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.CreateAt, _ => DateOnly.FromDateTime(DateTime.Now));

        }
    }
}
