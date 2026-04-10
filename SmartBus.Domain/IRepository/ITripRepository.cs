using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.IRepository
{
    public interface ITripRepository:IRepository<Trip>
    {
        public Task<Trip> GetWithDetails(Guid id);
        public Task<List<Trip>> GetAllWithDetails(int? StartLocationId, int? EndLocationId, DateTime? DepartureTime,DateTime? ArrivalTime);
    }
}
