using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class HallTypeRepository(APIContext context) : GenericRepository<HallType>(context), IHallTypeRepository
    {
        public sealed override async Task<IEnumerable<HallType>> GetAllAsync(Func<IQueryable<HallType>, IQueryable<HallType>>? query = null)
        {
            var _table = _context.HallType
                        .Include(ht => ht.Equipments);

            return await (query != null ? query(_table).ToListAsync() : _table.ToListAsync());
        }
        public sealed override async Task<HallType?> GetOneAsync(int id)
        {
            return await _context.HallType
                        .Include(ht => ht.Equipments)
                        .FirstOrDefaultAsync(ht => ht.Id == id);
        }
    }
}
