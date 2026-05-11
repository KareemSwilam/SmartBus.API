using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IExternalServices
{
    public interface ISendingEmailService
    {
        public Task<bool> SendingEmail(string to, string subject, string body);
    }
}
