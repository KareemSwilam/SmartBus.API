using MapsterMapper;
using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Services
{
    public class TripServices : ITripServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public TripServices(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        public async Task<CustomResult<TripDto>> AddTrip(CreateTripDto dto)
        {
            var CompanyExist = await _unit.CompanyRepository.Get(C => C.Id == dto.CompanyId);   
            if(CompanyExist == null)
                return CustomResult<TripDto>.Failure(CustomError.NotFound("Company Not Found"));
            var DriverExist = await _unit.DervierRepository.Get(D => D.Id == dto.DervierId && D.CompanyId == dto.CompanyId);
            if (DriverExist == null)
                return CustomResult<TripDto>.Failure(CustomError.NotFound("Deriver Not Found"));
            var BusExist = await _unit.BusRepository.Get(B => B.Id == dto.BusId && B.CompanyId == dto.CompanyId);
            if (BusExist == null)
                return CustomResult<TripDto>.Failure(CustomError.NotFound("Bus Not Found"));        
            var StartLocationExist = await _unit.LocationRepository.Get(L => L.Id == dto.StartLocationId);  
            if (StartLocationExist == null)
                return CustomResult<TripDto>.Failure(CustomError.NotFound("Start Location Not Found"));
            var EndLocationExist = await _unit.LocationRepository.Get(L => L.Id == dto.EndLocationId);
            if (EndLocationExist == null)
                return CustomResult<TripDto>.Failure(CustomError.NotFound("End Location Not Found"));
            var Trip = _mapper.Map<Trip>(dto);
            await _unit.TripRepository.Add(Trip);
            var complete = await _unit.SaveAsync();
            if (complete<1)
                return CustomResult<TripDto>.Failure(CustomError.ServerError("Failed To Add Trip"));
            var TripDto = _mapper.Map<TripDto>(Trip);
            return CustomResult<TripDto>.Success(TripDto);
        }

        public async Task<CustomResult<List<TripDto>>> GetAllTrips(TripSearchDto searchDto)
        {
            var Trips = await _unit.TripRepository.GetAllWithDetails(searchDto.StartLocationId, searchDto.EndLocationId, searchDto.DepartureTime, searchDto.ArrivalTime);
            var TripsDto = _mapper.Map<List<TripDto>>(Trips);
            return CustomResult<List<TripDto>>.Success(TripsDto);   
        }

        public async Task<CustomResult<TripDetailsDto>> GetTrip(Guid Id)
        {
            var Trip = await _unit.TripRepository.GetWithDetails(Id);
            if (Trip == null)
                return CustomResult<TripDetailsDto>.Failure(CustomError.NotFound("Trip Not Found"));
            var TripDto = _mapper.Map<TripDetailsDto>(Trip);   
            return CustomResult<TripDetailsDto>.Success(TripDto);
        }
    }
}
