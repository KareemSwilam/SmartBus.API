using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.IServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatServices _seatServices;
        public SeatController(ISeatServices seatServices)
        {
            _seatServices = seatServices;
        }
        [HttpGet("AvailableSeats")]
        public async Task<IActionResult> AvailableSeatsInTrip(Guid tripId, int? startLocationId, int? endLocationId)
        {
            var result = await _seatServices.AvailableSeatsInTrip(tripId, startLocationId, endLocationId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("ReservedSeats")]
        public async Task<IActionResult> ReservedSeatsInTrip(Guid tripId, int? startLocationId, int? endLocationId)
        {
            var result = await _seatServices.ReservedSeatsInTrip(tripId, startLocationId, endLocationId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
