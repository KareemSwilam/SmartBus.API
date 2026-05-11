using MapsterMapper;
using Microsoft.VisualBasic;
using SmartBus.Application.Dtos.CompanyDtos;
using SmartBus.Application.HelperMethod.EmailBodyBuilder;
using SmartBus.Application.IExternalServices;
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
        private readonly ISendingEmailService _sendingemail;
        private readonly IUserServices _userServices;
        
        public CompanyServices(IUnitOfWork unit, IMapper mapper, ISendingEmailService sendingemail, IUserServices userServices)
        {
             _unit = unit;
            _mapper = mapper;
            _sendingemail = sendingemail;
            _userServices = userServices;
        }

        public async Task<CustomResult<CompanyDto>> AddCompany(string userId, CreateCompanyDto dto)
        {
            var User = await _userServices.User(userId);
            if (!User.IsSuccess)
                return CustomResult<CompanyDto>.Failure(CustomError.InvalidInput("You Can't Adding New Company"));
            var company = _mapper.Map<Company>(dto);
            
            var result = await _unit.CompanyRepository.Add(company);
            var complete = await _unit.SaveAsync();
            User.Value!.CompanyId = result.Id;
            await _userServices.UpdateUser(userId, User.Value);            
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
        public async Task<CustomResult<List<CompanyDto>>> Companies()
        {
            var companies = await _unit.CompanyRepository.GetAll(c => !c.IsBlocked);
            var result = _mapper.Map<List<CompanyDto>>(companies);
            return CustomResult<List<CompanyDto>>.Success(result);  

        }
        public async Task<CustomResult<List<CompanyDto>>> BlockedCompanies()
        {
            var companies = await _unit.CompanyRepository.GetAll(c => c.IsBlocked);
            var result = _mapper.Map<List<CompanyDto>>(companies);
            return CustomResult<List<CompanyDto>>.Success(result);
        }
        public async Task<CustomResult> BlockCompany(Guid id)
        {
            var companyExist = await _unit.CompanyRepository.Get(c => c.Id == id);
            if (companyExist == null)
                CustomResult.Failure(CustomError.NotFound("Company You try To Block Not Exist"));
            companyExist!.IsBlocked = true;
            _unit.CompanyRepository.Update(companyExist);   
            await _unit.SaveAsync();
            await SendingBlokedMail(companyExist);
            return CustomResult.Success();
        }

        public async Task<CustomResult> UnBlockCompany(Guid id)
        {
            var companyExist = await _unit.CompanyRepository.Get(c => c.Id == id);
            if (companyExist == null)
                CustomResult.Failure(CustomError.NotFound("Company You try To Block Not Exist"));
            companyExist!.IsBlocked = false;
            _unit.CompanyRepository.Update(companyExist);
            await _unit.SaveAsync();
            return CustomResult.Success();

        }
        public async Task<CustomResult> DeleteCompany(string userId, Guid id)
        {
            var companyExist = await _unit.CompanyRepository.Get(c => c.Id == id);
            if (companyExist == null)
                CustomResult.Failure(CustomError.NotFound("Company You try To Block Not Exist"));
            var UserExist = await _userServices.User(userId);
            if (UserExist.Value!.CompanyId != id)
                CustomResult.Failure(CustomError.InvalidInput("You Can't Delete This Company"));
            _unit.CompanyRepository.Delete(companyExist!);
            await _unit.SaveAsync();
            return CustomResult.Success();
        }
        private async Task SendingBlokedMail(Company company)
        {
            var body = EmailBodyBlockedCompanyBuilder.BlockedCompanyBody(company.Name, DateOnly.FromDateTime(DateTime.UtcNow), "Outstanding invoice or policy violation");
            await _sendingemail.SendingEmail(company.Email, $"URGENT: Account Status Change for {company.Name}", body);
        }

    }
}
