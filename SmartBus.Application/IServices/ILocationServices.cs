using SmartBus.Application.Dtos.LocationDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface ILocationServices
    {
        public Task<CustomResult<List<LocationDto>>>  GetLocationsByName(string name);
    }
}
