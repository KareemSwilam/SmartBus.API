using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Dervier
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
        public Guid CompanyId { get; set; }
        public IEnumerable<Trip> Trips { get; set; }
        public Company Company { get; set; }   
    }
}
