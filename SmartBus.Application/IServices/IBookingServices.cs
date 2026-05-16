using SmartBus.Application.Dtos.BookingDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface IBookingServices
    {
        public Task<CustomResult> BookingSeat(BookingRequestDto requestDto, string userId);
    }
}
