using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.NotificationDtos
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        
        public string TargetId { get; set; }
        public NotificationTargetType TargetType { get; set; }
    }
}
