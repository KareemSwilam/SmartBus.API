using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.ExternalServices.PaymentExternalServices
{
    public class FawaterekPaymentSetting
    {
        public const string Name = "FawaterekPayment";
        public  string APIKey { get; set; }
        public string ProviderKey { get; set; }
        public string BaseURL { get; set; }
    }
}
