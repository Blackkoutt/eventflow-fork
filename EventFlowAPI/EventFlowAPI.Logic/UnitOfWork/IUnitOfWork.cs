using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.UnitOfWork
{
    public interface IUnitOfWork
    {
        // TODO (block 7): remove once every service uses its own specific repository interface
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IUserRepository Users { get; }

        IAdditionalServicesRepository AdditionalServices { get; }
        IEquipmentRepository Equipments { get; }
        IEventCategoryRepository EventCategories { get; }
        IEventPassTypeRepository EventPassTypes { get; }
        IHallTypeRepository HallTypes { get; }
        ISeatTypeRepository SeatTypes { get; }
        ITicketTypeRepository TicketTypes { get; }
        IPartnerRepository Partners { get; }
        INewsRepository News { get; }
        IFAQRepository FAQs { get; }
        IHallRepository Halls { get; }
        ISeatRepository Seats { get; }
        IEventRepository Events { get; }
        IFestivalRepository Festivals { get; }
        ITicketRepository Tickets { get; }

        Task SaveChangesAsync();

        /// <summary>
        /// Starts a transaction for changes that span several records or collections
        /// (a SQL transaction for the relational databases, a session for MongoDB).
        /// </summary>
        Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
