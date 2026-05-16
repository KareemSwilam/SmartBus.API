using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Application.Result;
using SmartBus.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface ITripServices
    {
        public Task<CustomResult<TripDto>> AddTrip(CreateTripDto dto);
        public Task<CustomResult<TripDetailsDto>> GetTrip(Guid Id);
        public Task<CustomResult<PaginationResult<TripWithStops>>> GetAllTrips(TripPaginationParams @params);
        public Task<CustomResult> GetFreeSeat(Guid TripId);

    }
}
