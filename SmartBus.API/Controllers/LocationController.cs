using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.IServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationServices _locationServices;
        public LocationController(ILocationServices locationServices)
        {
            _locationServices = locationServices;
        }
        [HttpGet("Locations")]
        public async Task<IActionResult> Locations(string? name)
        {
            var result = await _locationServices.GetLocations(name);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }
    }
}
