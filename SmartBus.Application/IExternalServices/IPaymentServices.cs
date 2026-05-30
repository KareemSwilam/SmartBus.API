using SmartBus.Application.Dtos.PaymentMethodDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IExternalServices
{
    public interface IPaymentServices
    {
        Task<CustomResult<EInvoicesResponseData>> CreateEInvoice(EInvoicesRequest eInvoices);   
        Task<CustomResult<List<PaymentMethodResponseData>>> GetPaymentMethods();
        CustomResult VerifyWebhook(WebHookResponseDto responseDto);
        CustomResult Cancelwebhook(CancelWebHookResponseDto responseDto);
    }
}
