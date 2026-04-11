using Microsoft.EntityFrameworkCore;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
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

        public async Task<List<Trip>> GetAllWithDetails(int? StartLocationId, int? EndLocationId, DateTime? DepartureTime, DateTime? ArrivalTime)
        {
            IQueryable<Trip>  trip = _dbSet;
            if(StartLocationId > 0)
                trip = trip.Where(t => t.StartLocationId == StartLocationId);
            
            if(EndLocationId > 0)
                trip = trip.Where(t => t.EndLocationId == EndLocationId);
            if (DepartureTime != null)
                trip = trip.Where(t => t.DepartureTime <= DepartureTime.Value);
            if (ArrivalTime != null)
                trip = trip.Where(t => t.ArrivalTime >= ArrivalTime.Value);
            trip = trip.Include(t => t.StartLocation)
                       .Include(t => t.EndLocation);

            return await trip.ToListAsync();
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
                                   .ThenInclude(ts => ts.Location).FirstOrDefaultAsync();
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
