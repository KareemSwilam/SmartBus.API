using SmartBus.Application.Dtos.LocationDtos;
using SmartBus.Domain.Enums;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.TripDtos
{
    public class TripDto
    {
        public LocationDto StartLocation { get; set; }
        public LocationDto EndLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
               
        public double Price { get; set; }
        public double AVGRating { get; set; }
        public TripStatus Status { get; set; }
    }
}
