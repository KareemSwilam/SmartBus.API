using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.HelperMethod.Notification
{
    public static class CreateActionUrl
    {
        public static string CreateUrl(NotificationTargetType type, string targetId)
        {
            switch (type)
            {
                case NotificationTargetType.Booking:
                    return $"booking/{targetId}";
                case NotificationTargetType.Trip:
                    return $"trip/{targetId}";
                case NotificationTargetType.Ticket:
                    return $"ticket/{targetId}";
                case NotificationTargetType.Payment:
                    return $"payment/{targetId}";
                default:
                    throw new ArgumentException("Invalid notification target type");
            }
        }
    }
}
