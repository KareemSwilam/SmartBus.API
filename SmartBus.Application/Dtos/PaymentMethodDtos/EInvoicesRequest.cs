using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.PaymentMethodDtos
{
    public class EInvoicesRequest
    {
        [JsonProperty("customer")]
        [Required]
        public CustomerModel Customer { get; set; }
        [JsonProperty("cartItems")]
        [MinLength(1)]
        [Required]

        public List<CartItemModel> Items { get; set; }
        [JsonProperty("cartTotal")]
        
        public decimal CartTotal =>  Items.Sum(i => i.Price * i.Quantity);
        [JsonProperty("currency")]
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = "EGP";
        [JsonProperty("sendEmail")]
        public bool SendEmail { get; set; } = true;
        [JsonProperty("redirectionUrls")]
        public RedirectionUrls RedirectionUrls { get; set; }
         

    }
    public class CustomerModel
    {
        [JsonProperty("first_name")]
        [Required]
        public  string FirstName { get; set; }
        [Required]
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string? email { get; set; }
        [JsonProperty("phone")]
        public int Phone { get; set; }
        [JsonProperty("customer_unique_id")]
        public string CustomerId { get; set; }
    }
    public class CartItemModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
    public class EInvoicesResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("data")]
        public EInvoicesResponseData Data { get; set; }
    }
    public class EInvoicesResponseData
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }
        [JsonProperty("invoiceKey")]
        public string InvoiceKey { get; set; }
    }
    public class RedirectionUrls
    {
        [JsonProperty("successUrl")]
        public string Success { get; set; }
        [JsonProperty("failUrl")]
        public string Failure { get; set; }
        
    }

}
