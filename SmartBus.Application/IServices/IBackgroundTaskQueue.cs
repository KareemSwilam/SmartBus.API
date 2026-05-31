using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface IBackgroundTaskQueue
    {
        void QueueBookingTicket(int bookingId);
        Task<int> DequeueAsync(CancellationToken cancellationToken);
    }
}
