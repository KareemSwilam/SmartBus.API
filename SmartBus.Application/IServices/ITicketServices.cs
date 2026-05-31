using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface ITicketServices
    {
        Task GenerateAndSendTicketAsync(int bookingId);
        Task<CustomResult<string>> ValidateTicket(int bookingId);
    }
}
