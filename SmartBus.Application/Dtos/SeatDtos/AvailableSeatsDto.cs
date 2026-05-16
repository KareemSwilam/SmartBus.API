using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.SeatDtos
{
    public class AvailableSeatsDto
    {
        public int Count { get; set; }
        public List<SeatDto> Seats { get; set; }
    }
}
