using SmartBus.Application.Dtos.NotificationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface INotifyServices
    {
        public Task SendNotificationToUser(string userId, SendNotificationDto dto);
        public Task SendNotificationToGroup(string groupName, SendNotificationDto dto);
        public Task SendNotificationToAll(SendNotificationDto dto);
    }
}
