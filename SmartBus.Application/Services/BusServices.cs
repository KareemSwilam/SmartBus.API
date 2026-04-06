using MapsterMapper;
using SmartBus.Application.Dtos.BusDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.Enums;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Services
{
    public class BusServices : IBusServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BusServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CustomResult> AddingBus(CreateBusDto createBusDto)
        {
            var company = await _unitOfWork.CompanyRepository.Get(c => c.Id ==createBusDto.CompanyId);
            if(company == null)
                return CustomResult.Failure(CustomError.NotFound("Company not found"));
               
            var bus =  _mapper.Map<Bus>(createBusDto);
            await _unitOfWork.BusRepository.Add(bus);
            await _unitOfWork.SaveAsync();
            
            
            for(int i = 1; i< createBusDto.SeatCount+1 ; i++)
            {
                var seat = new Seat
                { 
                    BusId = bus.Id,
                    SeatNumber = i,
                };
                await _unitOfWork.SeatRepository.Add(seat);
            }
            
            
            await _unitOfWork.SaveAsync();
            return CustomResult.Success();
        }

        public async  Task<CustomResult> DeletingBus(int id)
        {
            var bus = await _unitOfWork.BusRepository.Get(b => b.Id == id);
            if(bus == null)
                return CustomResult.Failure(CustomError.NotFound("Bus not found"));
            _unitOfWork.BusRepository.Delete(bus);    
            await _unitOfWork.SaveAsync();
            return CustomResult.Success();
        }

        public async Task<CustomResult<BusDto>> GetBus(int id)
        {
            var bus = await _unitOfWork.BusRepository.Get(b => b.Id == id);
            if(bus == null)
                return CustomResult<BusDto>.Failure(CustomError.NotFound("Bus not found"));
            var busDto = _mapper.Map<BusDto>(bus);
            return CustomResult<BusDto>.Success(busDto);
        }

        public async Task<CustomResult<List<BusDto>>> GetCompanyBuses(Guid companyId)
        {
            var bus = await _unitOfWork.BusRepository.GetAll(b => b.CompanyId == companyId);
            if (bus == null)
                return CustomResult<List<BusDto>>.Failure(CustomError.NotFound("Bus not found"));
            var busDto = _mapper.Map<List<BusDto>>(bus);
            return CustomResult<List<BusDto>>.Success(busDto);
        }
    }
}
