using MapsterMapper;
using SmartBus.Application.Dtos.DriverDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Services
{
    public class DriverServices : IDriverServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;   
        public DriverServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CustomResult<CreateDriverDto>> AddingDriver(Guid companyId, CreateDriverDto dto)
        {
            var companyExist = await  _unitOfWork.CompanyRepository.Get(c => c.Id == companyId);
            if (companyExist == null)
               return CustomResult<CreateDriverDto>.Failure(CustomError.NotFound("Company not found"));
            
            var driver = _mapper.Map<Driver>(dto);  
            driver.CompanyId = companyId;
            await _unitOfWork.DervierRepository.Add(driver);
            var result = _mapper.Map<CreateDriverDto>(driver);
            var complete = await _unitOfWork.SaveAsync();
            if(complete == 1)
                return CustomResult<CreateDriverDto>.Success(result);

            return CustomResult<CreateDriverDto>.Failure(CustomError.ServerError("Faild in Adding New Driver"));
        }

        public async Task<CustomResult> DeleteDriver(Guid driverId)
        {
            var driverExist =  await _unitOfWork.DervierRepository.Get(d => d.Id == driverId);
            if (driverExist == null)
                return CustomResult.Failure(CustomError.NotFound("Driver not found"));
            _unitOfWork.DervierRepository.Delete(driverExist);
            var complete =  _unitOfWork.SaveAsync().Result;
            if (complete == 1)
                return CustomResult.Success();
            return CustomResult.Failure(CustomError.ServerError("Faild in Deleting Driver"));

        }

        public async Task<CustomResult<List<DriverDto>>> GetAllDriversInCompany(Guid CompanyId)
        {
            var companyExist = await _unitOfWork.CompanyRepository.Get(c => c.Id == CompanyId);
            if (companyExist == null)
                return CustomResult<List<DriverDto>>.Failure(CustomError.NotFound("Company not found"));
            var Drivers = await _unitOfWork.DervierRepository.GetAll(d => d.CompanyId == CompanyId);
            var result = _mapper.Map<List<DriverDto>>(Drivers);
            return CustomResult<List<DriverDto>>.Success(result);
        }

        public async Task<CustomResult<DriverDto>> GetDriver(Guid driverId)
        {
            var driverExist = await _unitOfWork.DervierRepository.Get(d => d.Id == driverId);
            if(driverExist == null)
                return CustomResult<DriverDto>.Failure(CustomError.NotFound("Driver not found"));
            var result = _mapper.Map<DriverDto>(driverExist);
            return CustomResult<DriverDto>.Success(result);

        }

        public Task<CustomResult> GetDriverWithHisTrip(Guid driverId)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomResult<DriverDto>> UpdateDriver(Guid driverId, UpdateDriverDto dto)
        {
            var driverExist = await _unitOfWork.DervierRepository.Get(d => d.Id == driverId);
            if (driverExist == null)
                return CustomResult<DriverDto>.Failure(CustomError.NotFound("Driver not found"));
            var driver = _mapper.Map(dto, driverExist);
            driver.Id = driverId;   
            _unitOfWork.DervierRepository.Update(driver);
            var result = _mapper.Map<DriverDto>(driver);
            var complete = await _unitOfWork.SaveAsync();
            if (complete == 1)
                return CustomResult<DriverDto>.Success(result);
            return CustomResult<DriverDto>.Failure(CustomError.ServerError("Faild in Updating Driver"));
        }
    }
}
