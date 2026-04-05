using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.Dtos.DriverDtos;
using SmartBus.Application.IServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverServices _driverServices;
        public DriverController(IDriverServices driverServices)
        {
            _driverServices = driverServices;
        }
        [HttpPost("AddDriver/{companyId}")]
        public async Task<IActionResult> AddDriver(Guid companyId, [FromBody] CreateDriverDto dto)
        {
            var result = await _driverServices.AddingDriver(companyId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetDriver/{driverId}")]
        public async Task<IActionResult> GetDriver(Guid driverId)
        {
            var result = await _driverServices.GetDriver(driverId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetAllDriversInCompany/{companyId}")]
        public async Task<IActionResult> GetAllDriversInCompany(Guid companyId)
        {
            var result = await _driverServices.GetAllDriversInCompany(companyId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("UpdateDriver/{driverId}")]
        public async Task<IActionResult> UpdateDriver(Guid driverId, [FromBody] UpdateDriverDto dto)
        {
            var result = await _driverServices.UpdateDriver(driverId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("DeleteDriver/{driverId}")]
        public async Task<IActionResult> DeleteDriver(Guid driverId)
        {
            var result = await _driverServices.DeleteDriver(driverId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
