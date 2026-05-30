using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.Dtos.PaymentMethodDtos;
using SmartBus.Application.IExternalServices;
using SmartBus.Application.IServices;
using System.Threading.Tasks;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IBookingServices _booking;
        public PaymentController(IPaymentServices paymentServices, IBookingServices booking)
        {
            _paymentServices = paymentServices;
            _booking = booking;
        }
        [HttpPost]
        public async Task<IActionResult> EInvoice([FromBody] EInvoicesRequest eInvoices)
        {
            var result = await _paymentServices.CreateEInvoice(eInvoices);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var result = await _paymentServices.GetPaymentMethods();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("webhook_json")]
        [AllowAnonymous]
        public async Task<IActionResult> WebHook([FromBody] WebHookResponseDto responseDto)
        {
            var verify = _paymentServices.VerifyWebhook(responseDto);
            if (!verify.IsSuccess)
                return BadRequest(verify);
            var result = await _booking.HandlePaymentWebhook(responseDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("refund_json")]
        [AllowAnonymous]
        public IActionResult RefundWebHook([FromBody] CancelWebHookResponseDto responseDto)
        {
            var result = _paymentServices.Cancelwebhook(responseDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
