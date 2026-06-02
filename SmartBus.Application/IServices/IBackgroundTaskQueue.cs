using SmartBus.Application.Dtos.NotificationDtos;
using SmartBus.Domain.Models;
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
        void QueueNotification(CreateNotificationDto notification);
        Task<int> DequeueAsync(CancellationToken cancellationToken);
        Task<CreateNotificationDto> DequeueNotificationAsync(CancellationToken cancellationToken);
    }
}
