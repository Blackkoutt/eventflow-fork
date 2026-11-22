using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Exceptions;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
using EventFlowAPI.Data.SqlServer.Repositories;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Data.SqlServer
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly APIContext _context;
        private readonly Dictionary<Type, IRepository> _repositories;
        public UnitOfWork(APIContext context) 
        {
            _context = context;

            _additionalServices = new AdditionalServicesRepository(_context);
            _equipments = new EquipmentRepository(_context);
            _eventCategories = new EventCategoryRepository(_context);
            _eventPasses = new EventPassRepository(_context);
            _eventPassTypes = new EventPassTypeRepository(_context);
            _events = new EventRepository(_context);
            _tickets = new TicketRepository(_context);
            _festivals = new FestivalRepository(_context);
            _hallRents = new HallRentRespository(_context);
            _halls = new HallRepository(_context);
            _hallTypes = new HallTypeRepository(_context);
            _reservations = new ReservationRepository(_context);
            _seats = new SeatRepository(_context);
            _seatTypes = new SeatTypeRepository(_context);
            _ticketTypes = new TicketTypeRepository(_context);
            _users = new UserRepository(_context);
            _reservation_Seats = new Reservation_SeatRepository(_context);
            _festival_Events = new Festival_EventRepository(_context);
            _news = new NewsRepository(_context);
            _partners = new PartnerRepository(_context);
            _faqs = new FAQRepository(_context);

            _repositories = InitRepositoriesDictionary();
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>()  where TEntity : class 
        {
            if (_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }
            throw new RepositoryNotExistException($"No existing repository for entity type {typeof(TEntity)}.");
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            //DbUpdateException
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return new SqlServerTransaction(transaction);
        }

        public IUserRepository Users { get { return _users; } }
        public IAdditionalServicesRepository AdditionalServices => _additionalServices;
        public IEquipmentRepository Equipments => _equipments;
        public IEventCategoryRepository EventCategories => _eventCategories;
        public IEventPassTypeRepository EventPassTypes => _eventPassTypes;
        public IHallTypeRepository HallTypes => _hallTypes;
        public ISeatTypeRepository SeatTypes => _seatTypes;
        public ITicketTypeRepository TicketTypes => _ticketTypes;
        public IPartnerRepository Partners => _partners;
        public INewsRepository News => _news;
        public IFAQRepository FAQs => _faqs;
        public IHallRepository Halls => _halls;
        public ISeatRepository Seats => _seats;

        // Repositories not yet moved to specific methods (blocks 4-6)
        private Dictionary<Type, IRepository> InitRepositoriesDictionary()
        {
            return new Dictionary<Type, IRepository>
            {
                { typeof(EventPass), _eventPasses },
                { typeof(Event), _events },
                { typeof(Ticket), _tickets },
                { typeof(Festival), _festivals },
                { typeof(HallRent), _hallRents },
                { typeof(Reservation), _reservations },
                { typeof(Reservation_Seat), _reservation_Seats},
                { typeof(Festival_Event), _festival_Events},
            };
        }

        private readonly IAdditionalServicesRepository _additionalServices;
        private readonly IEquipmentRepository _equipments;
        private readonly IEventCategoryRepository _eventCategories;
        private readonly IEventPassRepository _eventPasses;
        private readonly IEventPassTypeRepository _eventPassTypes;
        private readonly IEventRepository _events;
        private readonly ITicketRepository _tickets;
        private readonly IFestivalRepository _festivals;
        private readonly IHallRentRepository _hallRents;
        private readonly IHallRepository _halls;
        private readonly IHallTypeRepository _hallTypes;
        private readonly IReservationRepository _reservations;
        private readonly ISeatRepository _seats;
        private readonly ISeatTypeRepository _seatTypes;
        private readonly ITicketTypeRepository _ticketTypes;
        private readonly IUserRepository _users;
        private readonly IReservation_SeatRepository _reservation_Seats;
        private readonly IFestival_EventRepository _festival_Events;
        private readonly INewsRepository _news;
        private readonly IPartnerRepository _partners;
        private readonly IFAQRepository _faqs;
    }
}
