using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class RefundRequest
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public int BookingId { get; set; }
        public long InvoiceId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime RequestTime { get; set; }   
        public RefundRequestStatus Status { get; set; }
        public DateTime? CompleteTime { get; set; }
        public Booking Booking { get; set; }
        public Company Company { get; set; }
    }
}
