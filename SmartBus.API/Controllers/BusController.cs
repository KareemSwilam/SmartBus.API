using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.Dtos.BusDtos;
using SmartBus.Application.IServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : ControllerBase
    {
        private readonly IBusServices _busServices;
        public BusController(IBusServices busServices)
        {
            _busServices = busServices;
        }
        [HttpPost("AddingBus")]
        public async Task<IActionResult> AddingBus(CreateBusDto createBusDto)
        {
            var result = await _busServices.AddingBus(createBusDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("DeletingBus")]
        public async Task<IActionResult> DeletingBus(int id)
        {
            var result = await _busServices.DeletingBus(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetBus/{id}")]
        public async Task<IActionResult> GetBus(int id)
        {
            var result = await _busServices.GetBus(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetCompanyBuses/{companyId}")]
        public async Task<IActionResult> GetCompanyBuses(Guid companyId)
        {
            var result = await _busServices.GetCompanyBuses(companyId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
