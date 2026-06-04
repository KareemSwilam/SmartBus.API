using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.IRepository
{
    public interface IBookingRepository: IRepository<Booking>
    {
        Task<List<Booking>> GetUserBookingWithTrip(string userId);
        Task<Booking> GetBookingWithDetails(Expression<Func<Booking, bool>> filter);
        Task<List<Booking>> GetTripBookings(Guid tripId);
    }
}
