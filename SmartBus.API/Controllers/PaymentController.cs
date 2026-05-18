using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.Dtos.PaymentMethodDtos;
using SmartBus.Application.IExternalServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        public PaymentController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;

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
        public IActionResult WebHook([FromBody] WebHookResponseDto responseDto)
        {
            var result =  _paymentServices.WebHook(responseDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
