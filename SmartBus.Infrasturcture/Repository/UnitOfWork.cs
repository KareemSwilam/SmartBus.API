using SmartBus.Domain.IRepository;
using SmartBus.Infrasturcture.Persistence;
using SmartBus.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly ApplicationContext _context;
        public IBookingRepository BookingRepository { get; private set; }
        public IBusRepository BusRepository { get; private set;}

        public ICompanyRepository CompanyRepository { get; private set; }

        public IDervierRepository DervierRepository { get; private set; }

        public ILocationRepository LocationRepository { get; private set; }

        public ISeatRepository SeatRepository { get; private set; }

        public ITicketRepository TicketRepository { get; private set; }

        public ITripRepository TripRepository { get; private set; }

        public ITripStopRepository TripStopRepository { get; private set; }
        public IStopSegmentRepository StopSegmentRepository { get; private set; }
        public IReservedSeatRepository ReservedSeatRepository { get; private set; }
        public UnitOfWork(ApplicationContext context,
            IBookingRepository bookingRepository,
            IBusRepository busRepository, 
            ICompanyRepository companyRepository,
            IDervierRepository dervierRepository,
            ILocationRepository locationRepository,
            ISeatRepository seatRepository,
            ITicketRepository ticketRepository,
            ITripRepository tripRepository,
            ITripStopRepository tripStopRepository, 
            IStopSegmentRepository stopSegmentRepository,
            IReservedSeatRepository reservedSeatRepository
            )
        {
            BookingRepository = bookingRepository;
            BusRepository = busRepository;
            CompanyRepository = companyRepository;
            DervierRepository = dervierRepository;
            LocationRepository = locationRepository;
            SeatRepository = seatRepository;
            TicketRepository = ticketRepository;
            TripStopRepository = tripStopRepository;
            TripRepository = tripRepository;
            _context = context;
            StopSegmentRepository = stopSegmentRepository;
            ReservedSeatRepository = reservedSeatRepository;


        }
        public async Task<int> SaveAsync()
        {
           return  await _context.SaveChangesAsync();
        }
    }
}
