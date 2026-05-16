using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.IRepository
{
    public interface IUnitOfWork
    {
        public IBookingRepository BookingRepository { get; }
        public IBusRepository BusRepository { get; }
        public ICompanyRepository   CompanyRepository { get; }
        public IDervierRepository DervierRepository { get; }
        public ILocationRepository LocationRepository { get; }
        public ISeatRepository SeatRepository { get; }
        public ITicketRepository TicketRepository { get; }  
        public ITripRepository TripRepository { get; }
        public ITripStopRepository TripStopRepository { get; }
        public IStopSegmentRepository StopSegmentRepository { get; }
        public IReservedSeatRepository ReservedSeatRepository { get; }
        public Task<int> SaveAsync();
    }
}
