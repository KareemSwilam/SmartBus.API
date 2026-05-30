using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        public string? PayloadJson { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public BookingPayload? Payload =>
            string.IsNullOrEmpty(PayloadJson)
                ? null
                : System.Text.Json.JsonSerializer.Deserialize<BookingPayload>(PayloadJson);
        [JsonPropertyName("referenceNumber")]
        public string? ReferenceNumber { get; set; }
    }
    

}
