using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Exceptions;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
using EventFlowAPI.Logic.Repositories.Repositories;

namespace EventFlowAPI.Logic.UnitOfWork
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
            _eventDetails = new EventDetailsRepository(_context);
            _eventPasses = new EventPassRepository(_context);
            _eventPassTypes = new EventPassTypeRepository(_context);
            _events = new EventRepository(_context);
            _tickets = new TicketRepository(_context);
            _festivalDetails = new FestivalDetailsRepository(_context);
            _festivals = new FestivalRepository(_context);
            _hallRents = new HallRentRespository(_context);
            _halls = new HallRepository(_context);
            _hallTypes = new HallTypeRepository(_context);
            _hallDetails = new HallDetailsRepository(_context);
            _reservations = new ReservationRepository(_context);
            _seats = new SeatRepository(_context);
            _seatTypes = new SeatTypeRepository(_context);
            _ticketTypes = new TicketTypeRepository(_context);
            _usersData = new UserDataRepository(_context);
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

        public IUserRepository Users { get { return _users; } }

        public APIContext Context { get { return _context; } }

        private Dictionary<Type, IRepository> InitRepositoriesDictionary()
        {
            return new Dictionary<Type, IRepository>
            {
                { typeof(AdditionalServices), _additionalServices },
                { typeof(Equipment), _equipments },
                { typeof(EventCategory), _eventCategories },
                { typeof(EventDetails), _eventDetails },
                { typeof(EventPass), _eventPasses },
                { typeof(EventPassType), _eventPassTypes },
                { typeof(Event), _events },
                { typeof(Ticket), _tickets },
                { typeof(FestivalDetails), _festivalDetails },
                { typeof(Festival), _festivals },
                { typeof(HallRent), _hallRents },
                { typeof(Hall), _halls },
                { typeof(HallType),  _hallTypes },
                { typeof(HallDetails),  _hallDetails },
                { typeof(Reservation), _reservations },
                { typeof(Seat), _seats },
                { typeof(SeatType), _seatTypes },
                { typeof(TicketType), _ticketTypes },
                { typeof(UserData), _usersData },
                { typeof(Reservation_Seat), _reservation_Seats},
                { typeof(Festival_Event), _festival_Events},
                { typeof(News), _news},
                { typeof(Partner), _partners},
                { typeof(FAQ), _faqs},
            };
        }

        private readonly IAdditionalServicesRepository _additionalServices;
        private readonly IEquipmentRepository _equipments;
        private readonly IEventCategoryRepository _eventCategories;
        private readonly IEventDetailsRepository _eventDetails;
        private readonly IEventPassRepository _eventPasses;
        private readonly IEventPassTypeRepository _eventPassTypes;
        private readonly IEventRepository _events;
        private readonly ITicketRepository _tickets;
        private readonly IFestivalDetailsRepository _festivalDetails;
        private readonly IFestivalRepository _festivals;
        private readonly IHallRentRepository _hallRents;
        private readonly IHallRepository _halls;
        private readonly IHallDetailsRepository _hallDetails;
        private readonly IHallTypeRepository _hallTypes;
        private readonly IReservationRepository _reservations;
        private readonly ISeatRepository _seats;
        private readonly ISeatTypeRepository _seatTypes;
        private readonly ITicketTypeRepository _ticketTypes;
        private readonly IUserDataRepository _usersData;
        private readonly IUserRepository _users;
        private readonly IReservation_SeatRepository _reservation_Seats;
        private readonly IFestival_EventRepository _festival_Events;
        private readonly INewsRepository _news;
        private readonly IPartnerRepository _partners;
        private readonly IFAQRepository _faqs;
    }
}
