using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.BusDtos
{
    public class BusDto
    {
        public int Id { get; set; }
        public int NumberSeats { get; set; }    
        public string PlateNumber { get; set; }
        public string BusNumber { get; set; }
        public string Type { get; set; }
    }
}
