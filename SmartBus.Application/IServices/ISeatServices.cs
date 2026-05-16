using SmartBus.Application.Dtos.SeatDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface ISeatServices
    {
        public Task<CustomResult<AvailableSeatsDto>> AvailableSeatsInTrip(Guid tripId, int? startLocationId = 0, int? endLocationId = 0);
        public Task<CustomResult<ReservedSeatDto>> ReservedSeatsInTrip(Guid tripId, int? startLocationId = 0, int? endLocationId = 0);
    }
}
