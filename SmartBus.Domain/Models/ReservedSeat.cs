using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class ReservedSeat
    {
        public int Id { get; set; }
        public Guid TripId { get; set; }
        public string UserId { get; set; }
        public int BookingId { get; set; }
        public int BusId { get; set; }  
        public int SeatId { get; set; }
        public int StartStopOrder { get; set; }    
        public int EndStopOrder { get; set; }
        public Bus Bus { get; set; }
        public Trip Trip { get; set; }
        
        public Seat Seat { get; set; }
    }
}
