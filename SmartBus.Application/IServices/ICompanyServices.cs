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
        public Task<CustomResult<CompanyDto>> AddCompany(string userId, CreateCompanyDto dto);
        public Task<CustomResult> BlockCompany(Guid id);
        public Task<CustomResult> UnBlockCompany(Guid id);
        public Task<CustomResult> DeleteCompany(string userId, Guid id);
        public Task<CustomResult<List<CompanyDto>>> Companies();
        public Task<CustomResult<List<CompanyDto>>> BlockedCompanies();
    }
}
