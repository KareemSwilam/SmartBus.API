using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public Guid CompanyId { get; set; }
        public BusType Type { get; set; }
        public string PlateNumber { get; set; }
        public string BusNumber { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
        public IEnumerable<Trip> Trips { get; set; }
        public Company Company { get; set; }
    }
}
