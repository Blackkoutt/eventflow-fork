using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories
{
    public abstract class EntityRepository<T>(APIContext context) : IEntityRepository<T> where T : class
    {
        protected readonly APIContext _context = context;
        protected DbSet<T> Table => _context.Set<T>();

        public virtual async Task<T?> GetByIdAsync(int id) => await Table.FindAsync(id);
        public async Task AddAsync(T entity) => await Table.AddAsync(entity);
        public void Update(T entity) => Table.Update(entity);
        public void Delete(T entity) => Table.Remove(entity);

        protected static async Task<IReadOnlyList<T>> ToListAsync(IQueryable<T> query) => await query.ToListAsync();
    }
}
