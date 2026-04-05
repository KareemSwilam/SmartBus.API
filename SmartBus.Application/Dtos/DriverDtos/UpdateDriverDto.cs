using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.DriverDtos
{
    public class UpdateDriverDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
    }
}
