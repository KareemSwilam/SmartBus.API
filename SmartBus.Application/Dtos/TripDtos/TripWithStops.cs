using SmartBus.Application.Dtos.LocationDtos;
using SmartBus.Application.Dtos.TripStopsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.TripDtos
{
    public class TripWithStops
    {
        public LocationDto EndLocation { get; set; }
        public LocationDto StartLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string CompanyName { get; set; }
        public string DriverName { get; set; }
        public string BusPlateNumber { get; set; }
        public double Price { get; set; }
        public List<StopDto> Stops { get; set; }
    }
}
