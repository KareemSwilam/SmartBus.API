using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.TripStopsDtos
{
    public class AddingStopDto
    {
        public int StopOrder { get; set; }
        public int LocationId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepatureTime { get; set; }
    }
}
