using Microsoft.EntityFrameworkCore;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using SmartBus.Infrasturcture.Persistence;

namespace SmartBus.Infrasturcture.Repository
{
    public class ReservedSeatRepository : Repository<ReservedSeat>, IReservedSeatRepository
    {
        public ReservedSeatRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<List<int>> GetReservedSeatsIdInTrip(Guid tripId, int fromStopOrder, int toStopOrder)
        {
            IQueryable<ReservedSeat> reservedSeats = _dbSet.Where((rs =>
                                                                   rs.TripId == tripId &&
                                                                   rs.StartStopOrder < toStopOrder &&
                                                                   rs.EndStopOrder > fromStopOrder))
                                                            .AsNoTracking();
            return await reservedSeats.Select(rs => rs.Seat.Id).ToListAsync();
        }
        public async Task<List<ReservedSeat>> GetReservedSeatsIdInTripWithDetails(Guid tripId, int fromStopOrder, int toStopOrder)
        {
             var reservedSeats = await  _dbSet.Where((rs => rs.TripId == tripId &&
                                                            rs.StartStopOrder < toStopOrder &&
                                                            rs.EndStopOrder > fromStopOrder))
                                              .Include(rs => rs.Seat)
                                              .Include(rs => rs.Bus)
                                              .AsNoTracking().ToListAsync();
            return reservedSeats;
        }
    }
}
