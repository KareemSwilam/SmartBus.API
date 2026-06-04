using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.Dtos.BookingDtos;
using SmartBus.Application.IServices;
using System.Security.Claims;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingServices _booking;
        public BookingController(IBookingServices booking)
        {
            _booking = booking;
        }
        [HttpPost("Book")]
        [Authorize]
        public async Task<IActionResult> Book([FromBody] BookingRequestDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _booking.BookingSeat(dto, userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("CancelBooking/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _booking.CancelBooking(bookingId, userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("RefundRequest/{companyId}")]
        public async Task<IActionResult> RefundRequest(Guid companyId)
        {
            var result = await _booking.GetRefundRequest(companyId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("UserBooking")]
        [Authorize]
        public async Task<IActionResult> UserBooking()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _booking.GetUserBookings(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("Booking/{id}")]
        public async Task<IActionResult> Booking(int id)
        {
            var result = await _booking.GetBooking(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("TripBookings/{tripId}")]
        public async Task<IActionResult> TripBookings(Guid tripId)
        {
            var result = await _booking.TripBookings(tripId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
