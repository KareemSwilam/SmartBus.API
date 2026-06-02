using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace SmartBus.Application.Dtos.NotificationDtos
{
    public class SendNotificationDto
    {   
        public Guid NotificationId { get; set; }
        public string Message { get; set; }
        public bool  IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ActionUrl { get; set; }
    }
}
