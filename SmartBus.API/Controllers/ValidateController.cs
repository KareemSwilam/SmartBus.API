using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.IServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {
        private readonly ITicketServices _ticketServices;
        public ValidateController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }
        [HttpGet("validate/{bookingId}")]
        public async Task<IActionResult> ValidateTicket(int bookingId)
        {
            var result = await _ticketServices.ValidateTicket(bookingId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);    
        }
    }
}
