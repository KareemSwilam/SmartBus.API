    using SmartBus.Domain.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace SmartBus.Domain.Models
    {
        public class Trip
        {
            public Guid Id { get; set; }
            public int BusId { get; set; }
            public Guid DervierId { get; set; }
            public Guid CompanyId { get; set; }

            public DateTime DepartureTime { get; set; }
            public DateTime ArrivalTime { get; set; }
            public int StartLocationId { get; set; }
            public int EndLocationId { get; set; }
            public double Price { get; set; }
            public double AVGRating { get; set; }
            public TripStatus Status { get; set; }
            public IEnumerable<TripStop> TripStops { get; set; }
            public IEnumerable<Booking> Bookings { get; set; }
            public IEnumerable<Tickect> Tickects { get; set; }
            public IEnumerable<Review> Reviews { get; set; }
            public IEnumerable<ReservedSeat> ReservedSeats { get; set; }    
            public Location EndLocation { get; set; }
            public Location StartLocation { get; set; }
            public Bus Bus { get; set; }
            public Driver Dervier { get; set; }
            public Company Company { get; set; }
        }
    }
