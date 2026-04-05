using SmartBus.Application.Dtos.DriverDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface IDriverServices
    {
        public Task<CustomResult<CreateDriverDto>> AddingDriver(Guid companyId, CreateDriverDto dto);
        public Task<CustomResult<DriverDto>> GetDriver(Guid driverId);
        public Task<CustomResult<List<DriverDto>>> GetAllDriversInCompany(Guid CompanyId);
        public Task<CustomResult<DriverDto>> UpdateDriver(Guid driverId, UpdateDriverDto dto);
        public Task<CustomResult> DeleteDriver(Guid driverId);
        public Task<CustomResult> GetDriverWithHisTrip(Guid driverId);

    }
}
