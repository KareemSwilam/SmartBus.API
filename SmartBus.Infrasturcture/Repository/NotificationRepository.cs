using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using SmartBus.Infrasturcture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Repository
{
    public class NotificationRepository:Repository<Notification> , INotificationRepository
    {
        public NotificationRepository(ApplicationContext context) : base(context)
        {
            
        }
    }
}
