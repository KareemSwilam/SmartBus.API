using SmartBus.Application.Dtos.StopSegmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.TripStopsDtos
{
    public  class AddingStopRequestDto
    {
        public Guid TripId { get; set; }
        public List<AddingStopDto> Stops { get; set; }
        public List<AddTripSegmentsDto> Segments { get; set; }
    }
}
