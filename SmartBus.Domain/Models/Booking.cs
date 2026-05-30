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
        public Guid TripId { get; set; }
        public string UserId { get; set; }
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public DateTime BookingDate { get; set; }
        public double Price { get; set; }
        public BookingStatus Status { get; set; }
        public Trip Trip { get; set; }
        public Tickect Ticket { get; set; }    
        public int SeatNumber { get; set; }
        public long? InvoiceId { get; set; }
        public string? PaymentReferenceNumber { get; set; }
    }
}
