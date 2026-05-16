using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public IEnumerable<Trip> StartTripsHere { get; set; }
        public IEnumerable<Trip> EndTripsHere { get; set; }
    }
}
