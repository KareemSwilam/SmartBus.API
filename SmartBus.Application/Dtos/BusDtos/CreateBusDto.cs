using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.BusDtos
{
    public class CreateBusDto
    {
        public Guid CompanyId { get; set; }
        public BusType Type { get; set; }
        public string PlateNumber { get; set; }
        public string BusNumber { get; set; }
        public int SeatCount { get; set; }
    }
}
