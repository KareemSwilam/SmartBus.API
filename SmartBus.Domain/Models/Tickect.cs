using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Tickect
    {
        public int Id { get; set; }
        public int TripId { get; set; } 
        public int FromStopOrdere { get; set; }
        public int ToStopOrder { get; set; }
        public int BookingId { get; set; }
        public string SeatNumber { get; set; }
        public Booking Booking { get; set; }
    }
}
