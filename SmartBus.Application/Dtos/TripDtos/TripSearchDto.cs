using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.TripDtos
{
    public class TripSearchDto
    {
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public DateTime? DepartureTime { get; set; }        
        public DateTime? ArrivalTime { get; set; }
    }
}
