using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.PaymentMethodDtos
{
    public class CancelWebHookResponseDto
    {
        [JsonPropertyName("transactionId")]
        public long TransactionId { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("currency")]
        public string  Currency { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("reason")]
        public string Reason { get; set; }
        [JsonPropertyName("approvedAt")]
        public string ApprovedAt { get; set; }
    }
}
