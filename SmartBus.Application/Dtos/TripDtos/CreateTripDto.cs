using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.TripDtos
{
    public class CreateTripDto
    {
        public int BusId { get; set; }
        public Guid DriverId { get; set; }
        public Guid CompanyId { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public double Price { get; set; }
            
    }
}
