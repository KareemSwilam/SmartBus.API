using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.SeatDtos
{
    public class ReservedSeatDto
    {
        public Guid TripId { get; set; }
        public string PlateNumber { get; set; }
        public List<SeatWithReservedInfoDto> Seats { get; set; }

    }
}
