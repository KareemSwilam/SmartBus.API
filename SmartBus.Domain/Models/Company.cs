using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? LogoUrl { get; set; }
        public double AVGRating { get; set; }
        public DateOnly CreateAt { get; set; }
        public IEnumerable<Driver>  Derviers { get; set; }
        public IEnumerable<Bus> Buses { get; set; }
        public IEnumerable<Trip> Trips { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
