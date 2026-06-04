using MapsterMapper;
using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using SmartBus.Domain.ValueObject;
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
            var DriverExist = await _unit.DervierRepository.Get(D => D.Id == dto.DriverId && D.CompanyId == dto.CompanyId);
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
            if (dto.StartLocationId == dto.EndLocationId)
                return CustomResult<TripDto>.Failure(CustomError.InvalidInput("Start And End Location Cannot Be Same"));
            if (dto.DepartureTime >= dto.ArrivalTime)            
                return CustomResult<TripDto>.Failure(CustomError.InvalidInput("Arrival Time Must Be After Departure Time"));
           
            var Trip = _mapper.Map<Trip>(dto);
            await _unit.TripRepository.Add(Trip);
            
            var startStop = new TripStop
            {
                Trip = Trip,
                LocationId = Trip.StartLocationId,
                StopOrder = 1,
                DepatureTime = Trip.DepartureTime,
                IsStartStop = true,
                IsEndStop = false
            };

            var endStop = new TripStop
            {
                Trip = Trip,
                LocationId = Trip.EndLocationId,
                StopOrder = 2,
                ArrivalTime = Trip.ArrivalTime,
                IsStartStop = false,
                IsEndStop = true
            };
            await _unit.TripStopRepository.Add(startStop);
            await _unit.TripStopRepository.Add(endStop);
            var complete = await _unit.SaveAsync();
            if (complete<1)
                return CustomResult<TripDto>.Failure(CustomError.ServerError("Failed To Add Trip"));
            var TripDto = _mapper.Map<TripDto>(Trip);
            return CustomResult<TripDto>.Success(TripDto);
        }

        public async Task<CustomResult<PaginationResult<TripWithStops>>> GetAllTrips(TripPaginationParams @params)
        {
            var Trips = await _unit.TripRepository.GetAllWithDetails(@params.PageNumber,@params.PageSize,
                                                            @params.Search.StartLocationId, @params.Search.EndLocationId,
                                                            @params.Search.DepartureTime, @params.Search.ArrivalTime);
            var result = new PaginationResult<TripWithStops>(_mapper.Map<List<TripWithStops>>(Trips.Items), Trips.TotalCount, Trips.PageNumber, Trips.PageSize);
           
            return CustomResult<PaginationResult<TripWithStops>>.Success(result);   
        }

        public async Task<CustomResult<TripDetailsDto>> GetTrip(Guid Id)
        {
            var Trip = await _unit.TripRepository.GetWithDetails(Id);
            if (Trip == null)
                return CustomResult<TripDetailsDto>.Failure(CustomError.NotFound("Trip Not Found"));
            var TripDto = _mapper.Map<TripDetailsDto>(Trip);
            return CustomResult<TripDetailsDto>.Success(TripDto);
        }
        
        public Task<CustomResult> GetFreeSeat(Guid TripId)
        {
            throw new NotImplementedException();
        }
    }
}
