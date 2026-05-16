using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.StopSegmentDtos
{
    public class AddTripSegmentsDto
    {
        public int FromStopOrder { get; set; }

        public int ToStopOrder { get; set; }

        public double Price { get; set; }
    }
}
