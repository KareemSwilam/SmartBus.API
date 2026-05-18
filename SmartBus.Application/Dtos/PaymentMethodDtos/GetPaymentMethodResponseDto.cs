using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.PaymentMethodDtos
{
    public class GetPaymentMethodResponseDto
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("data")]
        public List<PaymentMethodResponseData> Data { get; set; }
    }
    public class  PaymentMethodResponseData 
    {

        [JsonProperty("paymentId")]
        public int PaymentId { get; set; }
        [JsonProperty("name_en")]
        public string EnglishName { get; set; }
        [JsonProperty("name_ar")]
        public string ArabicName { get; set; }
        [JsonProperty("redirect")]
        public bool Redirect { get; set; }
        [JsonProperty("logo")]
        public string LogoUrl { get; set; }

    }
}
