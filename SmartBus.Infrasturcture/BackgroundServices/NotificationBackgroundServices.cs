using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartBus.Application.Dtos.NotificationDtos;
using SmartBus.Application.HelperMethod.Notification;
using SmartBus.Application.IServices;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.BackgroundServices
{
    public class NotificationBackgroundServices : BackgroundService
    {
        private readonly IServiceProvider _servicesProvider;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public NotificationBackgroundServices(IServiceProvider servicesProvider, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _servicesProvider = servicesProvider;
            _backgroundTaskQueue = backgroundTaskQueue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var notification =
                        await _backgroundTaskQueue.DequeueNotificationAsync(stoppingToken);

                    using var scope = _servicesProvider.CreateScope();

                    var unitOfwork =
                        scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var notificationEntity = new Notification
                    {
                        UserId = notification.UserId,
                        Message = notification.Message,
                        CreatedAt = DateTime.UtcNow,
                        TargetType = notification.TargetType,
                        TargetId = notification.TargetId,
                        ActionUrl = CreateActionUrl.CreateUrl(notification.TargetType, notification.TargetId)
                    };
                    await unitOfwork.NotificationRepository.Add(notificationEntity);
                    await unitOfwork.SaveAsync();
                    var dto = new SendNotificationDto
                    {
                        NotificationId = notificationEntity.Id,
                        Message = notificationEntity.Message,
                        IsRead = notificationEntity.IsRead,
                        CreatedAt = notificationEntity.CreatedAt,
                        ActionUrl = notificationEntity.ActionUrl
                    };
                    
                    var notificationService =
                        scope.ServiceProvider.GetRequiredService<INotifyServices>();
                    
                    await notificationService.SendNotificationToUser(notification.UserId, dto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Background job failed: {ex}");
                }
            }
        }
    }
   
}
