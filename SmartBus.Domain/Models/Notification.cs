using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Message { get; set; }

        public bool IsRead => ReadAt!= null;

        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }

        public NotificationTargetType TargetType { get; set; } 

        public string TargetId { get; set; }   
        public string ActionUrl { get; set; }
    }
}
