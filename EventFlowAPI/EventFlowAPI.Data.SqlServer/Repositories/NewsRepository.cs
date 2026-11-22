using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class NewsRepository(APIContext context) : EntityRepository<News>(context), INewsRepository
    {
        public Task<IReadOnlyList<News>> GetPageAsync(NewsQuery query)
            => ToListAsync(Table.SortBy(query.SortBy, query.SortDirection)
                                .GetPage(query.PageNumber, query.PageSize));

        public Task<bool> TitleExistsAsync(string title, int? excludedId = null)
            => Table.AnyAsync(n => n.Id != excludedId && n.Title.ToLower() == title.ToLower());
    }
}
