using MapsterMapper;
using SmartBus.Application.Dtos.CompanyDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Services
{
    public class CompanyServices : IComapnyServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public CompanyServices(IUnitOfWork unit, IMapper mapper)
        {
             _unit = unit;
            _mapper = mapper;
        }

        public async Task<CustomResult<CompanyDto>> AddCompany(CreateCompanyDto dto)
        {
            var company = _mapper.Map<Company>(dto);
            var result = await _unit.CompanyRepository.Add(company);
            
            var complete = await _unit.SaveAsync();
            if (complete == 1)
                return CustomResult<CompanyDto>.Success(_mapper.Map<CompanyDto>(result));
            return CustomResult<CompanyDto>.Failure(CustomError.ServerError("Falid in adding new company"));

        }

        public async Task<CustomResult<CompanyDto>> GetCompanyById(Guid id)
        {
            var company = await _unit.CompanyRepository.Get(c => c.Id == id);
            if (company == null)
                return CustomResult<CompanyDto>.Failure(CustomError.NotFound("Company Not Found"));
            return  CustomResult<CompanyDto>.Success(_mapper.Map<CompanyDto>(company));
        }
    }
}
