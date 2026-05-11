using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.ExternalServices.MailExternalServices
{
    public class MailkitSetting
    {
        public const string Name = "MailKitSetting";
        public string From { get; set; }
        public string To { get; set; }
        public string apiKey { get; set; }
        public string UserName { get; set; }
        public int Port {  get; set; }
        public string Host { get; set; }
       
    }
}
