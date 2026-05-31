using SmartBus.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SmartBus.Application.Services
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<int> _queue;
        public BackgroundTaskQueue()
        {
            _queue = Channel.CreateUnbounded<int>();

        }
        public async Task<int> DequeueAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("background Try to get booking id from queue");
            return await _queue.Reader.ReadAsync(cancellationToken);
        }

        public void QueueBookingTicket(int bookingId)
        {
            _queue.Writer.TryWrite(bookingId);
            Console.WriteLine("recieving booking id from Booking Services ");
        }
    }
}
