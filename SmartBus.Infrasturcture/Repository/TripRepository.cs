using Microsoft.EntityFrameworkCore;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using SmartBus.Domain.ValueObject;
using SmartBus.Infrasturcture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Repository
{
    public class TripRepository:Repository<Trip>, ITripRepository
    {
        public TripRepository(ApplicationContext context):base(context)
        {
            
        }

        public async Task<PaginationResult<Trip>> GetAllWithDetails(int pageNumber, int pageSize,int? StartLocationId, int? EndLocationId, DateTime? DepartureTime, DateTime? ArrivalTime)
        {
            IQueryable<Trip>  trip = _dbSet;
            
            
            if (StartLocationId > 0)
                trip = trip.Where(t => t.StartLocationId == StartLocationId || t.TripStops.Any(ts => ts.LocationId == StartLocationId));
            
            if(EndLocationId > 0)
                trip = trip.Where(t => t.EndLocationId == EndLocationId || t.TripStops.Any(ts => ts.LocationId == EndLocationId));
            if (DepartureTime != null)
                trip = trip.Where(t => t.DepartureTime <= DepartureTime.Value);
            if (ArrivalTime != null)
                trip = trip.Where(t => t.ArrivalTime >= ArrivalTime.Value);
            var Count = trip.Count();
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
                             .Skip(pageSize*(pageNumber - 1))
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
        //public async Task<Trip> GetFreeSeats(Guid id, int? StartLocationId, int? EndLocationId)
        //{
        //    var query =  _dbSet.Where(t => t.Id == id);
        //    if(StartLocationId != 0)
        //    {

        //    }
        //}
    }
}
