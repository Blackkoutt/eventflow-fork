namespace EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces
{
    /// <summary>
    /// Operations every repository has. Changes are written by IUnitOfWork.SaveChangesAsync.
    /// Queries are not part of it: each repository declares the ones its use cases need.
    /// </summary>
    public interface IEntityRepository<T> : IRepository where T : class
    {
        /// <summary>Returns the record with the given id, also when it is soft-deleted.</summary>
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
