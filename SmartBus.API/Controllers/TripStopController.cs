using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.Dtos.TripStopsDtos;
using SmartBus.Application.IServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripStopController : ControllerBase
    {
        private readonly ITripStopsServices _tripStopsServices;
        public TripStopController(ITripStopsServices tripStopsServices)
        {
            _tripStopsServices = tripStopsServices;
        }
        [HttpPost("AddStop")]
        public async Task<IActionResult> AddingStop(AddingStopDto dto)
        {
            var result = await _tripStopsServices.AddingStop(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("TripWithStops")]
        public async Task<IActionResult> TripWithStops(Guid TripId)
        {
            var result = await _tripStopsServices.TripWithStops(TripId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
