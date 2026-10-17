using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Logic.Repositories.Repositories
{
    public sealed class HallRentRespository(APIContext context) : GenericRepository<HallRent>(context), IHallRentRepository
    {
        public sealed override async Task<IEnumerable<HallRent>> GetAllAsync(Func<IQueryable<HallRent>, IQueryable<HallRent>>? query = null)
        {
            var _table = _context.HallRent
                        .Include(hr => hr.User)
                        .Include(hr => hr.Hall)
                            .ThenInclude(h => h.Type)
                                 .ThenInclude(t => t.Equipments)
                        .Include(hr => hr.AdditionalServices)
                        .AsSplitQuery();

            return await (query != null ? query(_table).ToListAsync() : _table.ToListAsync());
        }
        
        public sealed override async Task<HallRent?> GetOneAsync(int id)
        {
            return await _context.HallRent
                        .AsSplitQuery()
                        .Include(hr => hr.User)
                        .Include(hr => hr.Hall)
                            .ThenInclude(h => h.Type)
                                .ThenInclude(t => t.Equipments)
                        .Include(hr => hr.AdditionalServices)
                        .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
