using SmartBus.Application.Dtos.TripDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.DriverDtos
{
    public class DriverWithTrips
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
        public double AVGRating { get; set; }
        public List<TripDto> Trips { get; set; }    
    }
}
