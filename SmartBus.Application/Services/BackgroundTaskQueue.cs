using SmartBus.Application.Dtos.NotificationDtos;
using SmartBus.Application.IServices;
using SmartBus.Domain.Models;
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
        private readonly Channel<int> _BookingQueue;
        private readonly Channel<CreateNotificationDto> _NotificationQueue;
        public BackgroundTaskQueue()
        {
            _BookingQueue = Channel.CreateUnbounded<int>();
            _NotificationQueue = Channel.CreateUnbounded<CreateNotificationDto>();
        }
        public async Task<int> DequeueAsync(CancellationToken cancellationToken)
        {
            
            return await _BookingQueue.Reader.ReadAsync(cancellationToken);
        }

        public async Task<CreateNotificationDto> DequeueNotificationAsync(CancellationToken cancellationToken)
        {
            
            return await _NotificationQueue.Reader.ReadAsync(cancellationToken);
        }

        public void QueueBookingTicket(int bookingId)
        {
            _BookingQueue.Writer.TryWrite(bookingId);
            
        }

        public void QueueNotification(CreateNotificationDto notification)
        {
            _NotificationQueue.Writer.TryWrite(notification);
            
        }
    }
}
