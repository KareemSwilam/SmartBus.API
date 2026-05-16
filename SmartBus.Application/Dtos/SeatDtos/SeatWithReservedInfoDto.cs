using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.SeatDtos
{
    public class SeatWithReservedInfoDto: SeatDto
    {
        public List<ReservedInfoDto> Reserveds { get; set; }
    }
}
