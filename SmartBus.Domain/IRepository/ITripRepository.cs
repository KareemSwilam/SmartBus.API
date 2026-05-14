using SmartBus.Domain.Models;
using SmartBus.Domain.ValueObject;
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
        public Task<PaginationResult<Trip>> GetAllWithDetails(int pageNumber, int pageSize, int? StartLocationId, int? EndLocationId, DateTime? DepartureTime,DateTime? ArrivalTime);
        public Task<Trip> GetTripWithStops(Guid id);
    }
}
