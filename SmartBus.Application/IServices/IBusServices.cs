using SmartBus.Application.Dtos.BusDtos;
using SmartBus.Application.Result;
using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface IBusServices
    {
        public Task<CustomResult> AddingBus(CreateBusDto createBusDto);
        public Task<CustomResult> DeletingBus(int id);
        public Task<CustomResult<BusDto>> GetBus(int id);
        public Task<CustomResult<List<BusDto>>> GetCompanyBuses(Guid companyId);
    }
}
