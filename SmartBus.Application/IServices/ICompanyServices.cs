using SmartBus.Application.Dtos.CompanyDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface IComapnyServices
    {
        public Task<CustomResult<CompanyDto>> GetCompanyById(Guid id);
        public Task<CustomResult<CompanyDto>> AddCompany(CreateCompanyDto dto);
    }
}
