using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Application.Dtos.TripStopsDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface ITripStopsServices
    {
        public Task<CustomResult<TripWithStops>> AddingStop(AddingStopDto dto);
    }
}
