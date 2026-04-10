using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SmartBus.Application.Dtos.TripDtos;
using SmartBus.Application.IServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripServices _tripServices;
        public TripController(ITripServices tripServices)
        {
            _tripServices = tripServices;
        }
        [HttpPost("AddingTrip")]
        public async Task<IActionResult> AddTrip(CreateTripDto dto)
        {
            var result = await _tripServices.AddTrip(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("GetTripDetails")]
        public async Task<IActionResult> GetTripDetails(Guid Id)
        {
            var result = await _tripServices.GetTrip(Id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("GetAllTrips")]
        public async Task<IActionResult> GetAllTrips([FromQuery] TripSearchDto searchDto)
        {
            var result = await _tripServices.GetAllTrips(searchDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
