using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.RefundRequestDtos
{
    public class RefundRequestDto
    {
        public int Id { get; set; }
        
        public int BookingId { get; set; }
        public long InvoiceId { get; set; }
        
        public DateTime RequestTime { get; set; }
        public string Status { get; set; }
        public DateTime? CompleteTime { get; set; }
    }
}
