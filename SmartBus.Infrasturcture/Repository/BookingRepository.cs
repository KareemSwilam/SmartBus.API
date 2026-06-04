using Microsoft.EntityFrameworkCore;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using SmartBus.Infrasturcture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Repository
{
    public class BookingRepository:Repository<Booking>, IBookingRepository  
    {
        public BookingRepository(ApplicationContext context) : base(context) { }

        public async Task<Booking> GetBookingWithDetails(Expression<Func<Booking, bool>> filter)
        {
            IQueryable<Booking> query = _dbSet.Where(filter)
                                              .Include(b => b.Trip).ThenInclude(t => t.StartLocation)
                                              .Include(b => b.Trip).ThenInclude(t => t.EndLocation)
                                              .Include(b => b.Trip).ThenInclude(t => t.Bus)
                                              .Include(b => b.Trip).ThenInclude(t => t.Company)
                                              .AsSplitQuery().AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Booking>> GetTripBookings(Guid tripId)
        {
            IQueryable<Booking> query = _dbSet.Where(b => b.TripId == tripId)
                                              .Include(b => b.Trip).ThenInclude(t => t.StartLocation)
                                              .Include(b => b.Trip).ThenInclude(t => t.EndLocation)
                                              .Include(b => b.Trip).ThenInclude(t => t.Bus)
                                              .Include(b => b.Trip).ThenInclude(t => t.Company)
                                              .AsSplitQuery().AsNoTracking();
            return await query.ToListAsync(); ;
        }

        public async Task<List<Booking>> GetUserBookingWithTrip(string userId)
        {
           IQueryable<Booking> query =  _dbSet.Where(b => b.UserId == userId)
                                              .Include(b => b.Trip).ThenInclude(t => t.StartLocation)
                                              .Include(b => b.Trip).ThenInclude(t => t.EndLocation)
                                              .Include(b => b.Trip).ThenInclude(t => t.Bus)
                                              .Include(b => b.Trip).ThenInclude(t => t.Company)
                                              .AsSplitQuery().AsNoTracking();
            return await query.ToListAsync();
        }
    }
}
