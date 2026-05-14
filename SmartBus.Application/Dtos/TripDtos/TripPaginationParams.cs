using SmartBus.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.TripDtos
{
    public class TripPaginationParams: PaginationRequestDto
    {
        public TripSearchDto Search { get; set; } = new TripSearchDto();
    }
}
