using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.TicketDtos
{
    public class GenerateTicketInfoDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }   
        public string CompanyName { get; set; }
        public string StartLocation { get; set; }
        public string Endlocation { get; set; }
        public string BusNumber { get; set; }
        public int SeatNumber { get; set; }  
        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

       
        public double PaymentAmount { get; set; }
    }
}
