using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Enums
{
    public enum  TripStatus
    {
        Scheduled = 1,     
        Boarding = 2,      
        InProgress = 3,    
        Completed = 4,     
        Cancelled = 5,     
        Delayed = 6
    }
}
