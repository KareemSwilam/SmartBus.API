using SmartBus.Domain.IRepository;
using SmartBus.Infrasturcture.Persistence;

namespace SmartBus.Infrasturcture.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationContext _context;
        public IBookingRepository BookingRepository { get; private set; }
        public IBusRepository BusRepository { get; private set; }

        public ICompanyRepository CompanyRepository { get; private set; }

        public IDervierRepository DervierRepository { get; private set; }

        public ILocationRepository LocationRepository { get; private set; }

        public ISeatRepository SeatRepository { get; private set; }

        public ITicketRepository TicketRepository { get; private set; }

        public ITripRepository TripRepository { get; private set; }

        public ITripStopRepository TripStopRepository { get; private set; }
        public IStopSegmentRepository StopSegmentRepository { get; private set; }
        public IReservedSeatRepository ReservedSeatRepository { get; private set; }
        public IRefundRequestRepository RefundRequestRepository { get; private set; }
        public INotificationRepository NotificationRepository { get; private set; }
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
            IReservedSeatRepository reservedSeatRepository,
            IRefundRequestRepository refundRequestRepository,
            INotificationRepository notificationRepository
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
            RefundRequestRepository = refundRequestRepository;
            NotificationRepository = notificationRepository;
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
