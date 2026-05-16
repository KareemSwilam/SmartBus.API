using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.IRepository
{
    public interface IReservedSeatRepository: IRepository<ReservedSeat>
    {
        public Task<List<int>> GetReservedSeatsIdInTrip(Guid tripId, int fromStopOrder, int toStopOrder);
        public Task<List<ReservedSeat>> GetReservedSeatsIdInTripWithDetails(Guid tripId, int fromStopOrder, int toStopOrder);
    }
}
