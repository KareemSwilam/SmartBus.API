using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class TripStop
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int LoctionId { get; set; }
        public int StopOrder { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime  DepatureTime { get; set; }
        public Trip Trip { get; set; }
    }
}
