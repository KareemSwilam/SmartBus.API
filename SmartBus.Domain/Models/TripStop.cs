using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class TripStop
    {
        public int Id { get; set; }
        public Guid TripId { get; set; }
        public int LocationId { get; set; }
        public int StopOrder { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime?  DepatureTime { get; set; }
        public Trip Trip { get; set; }
        public Location Location { get; set; } 
        public bool IsStartStop { get; set; }
        public bool IsEndStop { get; set; }
    }
}
