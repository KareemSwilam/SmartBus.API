using Azure;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using SmartBus.Application.Dtos.PaymentMethodDtos;
using SmartBus.Application.IExternalServices;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.ExternalServices.PaymentExternalServices
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FawaterekPaymentSetting _setting;
        

        public PaymentServices(IHttpClientFactory httpClientFactory, IOptions<FawaterekPaymentSetting> options)
        {
            _httpClientFactory = httpClientFactory;
            _setting = options.Value;
        }
        public async Task<CustomResult<EInvoicesResponseData>> CreateEInvoice(EInvoicesRequest eInvoices)
        {
            var client = _httpClientFactory.CreateClient();

            client.Timeout = TimeSpan.FromMinutes(5);

            var request = new HttpRequestMessage(HttpMethod.Post,$"{_setting.BaseURL}/createInvoiceLink");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _setting.APIKey);

            request.Content = new StringContent(JsonConvert.SerializeObject(eInvoices),Encoding.UTF8,"application/json");

            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine(responseContent);

            if (!response.IsSuccessStatusCode)
            {
                return CustomResult<EInvoicesResponseData>.Failure(
                    CustomError.ServerError(responseContent));
            }

            var result =
                JsonConvert.DeserializeObject<EInvoicesResponse>(responseContent);

            return CustomResult<EInvoicesResponseData>.Success(result!.Data);
        }
        
        public CustomResult Cancelwebhook(CancelWebHookResponseDto responseDto)
        {
            return CustomResult.Success();
        }
        public async Task<CustomResult<List<PaymentMethodResponseData>>> GetPaymentMethods()
        {
            var client = _httpClientFactory.CreateClient();

            client.Timeout = TimeSpan.FromMinutes(5);

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_setting.BaseURL}/getPaymentmethods");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _setting.APIKey);
            request.Content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine(responseContent);

            if (!response.IsSuccessStatusCode)
            {
                return CustomResult<List<PaymentMethodResponseData>>.Failure(
                    CustomError.ServerError(responseContent));
            }

            var result =
                JsonConvert.DeserializeObject<GetPaymentMethodResponseDto>(responseContent);

            return CustomResult<List<PaymentMethodResponseData>>.Success(result.Data);
        }
        public CustomResult VerifyWebhook(WebHookResponseDto webHook)
        {
            var generatedHashKey =
                GenerateHashKeyForWebhookVerification(webHook.InvoiceId, webHook.InvoiceKey, webHook.PaymentMethod);
            return generatedHashKey == webHook.HashKey ? CustomResult.Success() : CustomResult.Failure(CustomError.InvalidInput("Invalid webhook"));
        }
        private string GenerateHashKeyForWebhookVerification(long invoiceId, string invoiceKey, string paymentMethod)
        {
            var queryParam = $"InvoiceId={invoiceId}&InvoiceKey={invoiceKey}&PaymentMethod={paymentMethod}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_setting.APIKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryParam));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        
    }
}
