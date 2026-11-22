using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class TicketTypeRepository(APIContext context) : EntityRepository<TicketType>(context), ITicketTypeRepository
    {
        public Task<IReadOnlyList<TicketType>> GetPageAsync(TicketTypeQuery query)
            => ToListAsync(Table.Where(tt => !tt.IsDeleted)
                                .ByName(query)
                                .SortBy(query.SortBy, query.SortDirection)
                                .GetPage(query.PageNumber, query.PageSize));

        public Task<IReadOnlyList<TicketType>> GetActiveAsync()
            => ToListAsync(Table.Where(tt => !tt.IsDeleted));

        public Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null)
            => Table.AnyAsync(tt => tt.Id != excludedId && !tt.IsDeleted && tt.Name.ToLower() == name.ToLower());

        public Task<bool> IsUsedAsync(int id)
            => _context.Ticket.AnyAsync(t => t.TicketTypeId == id);
    }
}
