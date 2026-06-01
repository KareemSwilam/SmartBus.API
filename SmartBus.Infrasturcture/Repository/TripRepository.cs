using Microsoft.EntityFrameworkCore;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using SmartBus.Domain.ValueObject;
using SmartBus.Infrasturcture.Persistence;

namespace SmartBus.Infrasturcture.Repository
{
    public class TripRepository : Repository<Trip>, ITripRepository
    {
        public TripRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<PaginationResult<Trip>> GetAllWithDetails(int pageNumber, int pageSize, int? StartLocationId, int? EndLocationId, DateTime? DepartureTime, DateTime? ArrivalTime)
        {
            IQueryable<Trip> trip = _dbSet;


            if (StartLocationId > 0)
                trip = trip.Where(t => t.TripStops.Any(ts => ts.LocationId == StartLocationId));

            if (EndLocationId > 0)
                trip = trip.Where(t => t.TripStops.Any(ts => ts.LocationId == EndLocationId));
            if (StartLocationId > 0 && EndLocationId > 0)
            {
                trip = trip.Where(t =>
                                    t.TripStops.Where(ts => ts.LocationId == StartLocationId)
                                    .Min(ts => ts.StopOrder) < t.TripStops.Where(ts => ts.LocationId == EndLocationId)
                                    .Min(ts => ts.StopOrder));
            }
            if (DepartureTime != null)
                trip = trip.Where(t => t.DepartureTime <= DepartureTime.Value);
            if (ArrivalTime != null)
                trip = trip.Where(t => t.ArrivalTime >= ArrivalTime.Value);
            var Count = await trip.CountAsync();
            trip = trip.Include(t => t.StartLocation)
                       .Include(t => t.EndLocation)
                       .Include(t => t.Company)
                       .Include(t => t.Bus)
                       .Include(t => t.Dervier)
                       .Include(t => t.TripStops)
                       .ThenInclude(ts => ts.Location)
                       .AsSplitQuery()
                       .AsNoTracking();
            var result = await trip.OrderByDescending(t => t.DepartureTime)
                             .Skip(pageSize * (pageNumber - 1))
                             .Take(pageSize)
                             .ToListAsync();

            return new PaginationResult<Trip>(result, Count, pageNumber, pageSize);
        }

        public async Task<Trip> GetTripWithStops(Guid id)
        {
            var trip = await _dbSet.Where(t => t.Id == id)
                                   .Include(t => t.Bus)
                                   .Include(t => t.Dervier)
                                   .Include(t => t.Company)
                                   .Include(t => t.StartLocation)
                                   .Include(t => t.EndLocation)
                                   .Include(t => t.TripStops)
                                   .ThenInclude(ts => ts.Location)
                                   .AsSplitQuery()
                                   .AsNoTracking().FirstOrDefaultAsync();
            return trip;
        }

        public async Task<Trip> GetWithDetails(Guid id)
        {
            var trip = await _dbSet.Where(t => t.Id == id)
                                   .Include(t => t.Bus)
                                   .Include(t => t.Dervier)
                                   .Include(t => t.Company)
                                   .Include(t => t.StartLocation)
                                   .Include(t => t.EndLocation).FirstOrDefaultAsync();

            return trip;
        }

    }
}
