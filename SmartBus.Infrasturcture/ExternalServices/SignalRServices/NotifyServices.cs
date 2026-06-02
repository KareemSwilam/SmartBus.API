using Microsoft.AspNetCore.SignalR;
using SmartBus.Application.Dtos.NotificationDtos;
using SmartBus.Application.IServices;
using SmartBus.Infrasturcture.Hubs;

namespace SmartBus.Infrasturcture.ExternalServices.SignalRServices
{
    public class NotifyServices : INotifyServices
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotifyServices(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendNotificationToAll(SendNotificationDto dto)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", dto);
        }

        public async Task SendNotificationToGroup(string groupName, SendNotificationDto dto)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", dto);
        }

        public async Task SendNotificationToUser(string userId, SendNotificationDto dto)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", dto);
        }
    }
}
