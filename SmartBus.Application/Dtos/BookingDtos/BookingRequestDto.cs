using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.BookingDtos
{
    public class BookingRequestDto
    {
        public Guid TripId { get; set; }
        
        public int SeatId { get; set; }
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        
    }
}
