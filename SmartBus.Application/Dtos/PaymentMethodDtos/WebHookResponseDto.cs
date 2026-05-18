using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SmartBus.Application.Dtos.PaymentMethodDtos
{
    public class WebHookResponseDto
    {
        [JsonPropertyName("hashKey")]
        public string? HashKey { get; set; }
        [JsonPropertyName("invoice_id")]
        public long InvoiceId { get; set; }
        [JsonPropertyName("invoice_key")]
        public string? InvoiceKey { get; set; }
        [JsonPropertyName("payment_method")]
        public string? PaymentMethod { get; set; }
        [JsonPropertyName("invoice_status")]
        public string? InvoiceStatus { get; set; }
        [JsonPropertyName("pay_load")]
        public Payload? payload { get; set; }
        [JsonPropertyName("referenceNumber")]
        public string? ReferenceNumber { get; set; }
    }
    public class Payload
    {
    }

}
