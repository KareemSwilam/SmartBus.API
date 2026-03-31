using Microsoft.AspNetCore.Identity;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public bool IsCompany { get; set; }
        public Guid? CompanyId { get; set; }   
        public Company Company { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
