using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string UserId { get; set; }
        public int FromStopOrdere { get; set; }
        public int ToStopOrder { get; set; }
        public DateTime BookingDate { get; set; }
        public double Price { get; set; }
        public BookingStatus Status { get; set; }
        public Trip Trip { get; set; }
        public Tickect Tickect { get; set; }    
        public string SeatNumber { get; set; }
    }
}
