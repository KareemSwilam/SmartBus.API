using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class StopSegment
    {
        public int Id { get; set; }

        public Guid TripId { get; set; }

        public int FromStopOrder { get; set; }

        public int ToStopOrder { get; set; }

        public double Price { get; set; }

        public Trip Trip { get; set; }

        
    }
}
