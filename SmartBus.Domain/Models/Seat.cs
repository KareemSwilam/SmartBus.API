using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int BusId { get; set; }
        public int SeatNumber { get; set; }
        public Bus Bus { get; set; }

    }
}
