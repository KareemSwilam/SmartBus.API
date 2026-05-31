using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.BookingDtos
{
    public class UserBookingDto
    {
        public int BookingId { get; set; }
        public Guid TripId { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string CompanyName { get; set; }
        public string BusNumber { get; set; }
        public int SeatNumber { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
    }
}
