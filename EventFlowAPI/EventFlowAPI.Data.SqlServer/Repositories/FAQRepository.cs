using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class FAQRepository(APIContext context) : EntityRepository<FAQ>(context), IFAQRepository
    {
        public Task<IReadOnlyList<FAQ>> GetPageAsync(FAQQuery query)
            => ToListAsync(Table.SortBy(query.SortBy, query.SortDirection)
                                .GetPage(query.PageNumber, query.PageSize));

        public Task<bool> QuestionExistsAsync(string question, int? excludedId = null)
            => Table.AnyAsync(f => f.Id != excludedId && f.Question.ToLower() == question.ToLower());
    }
}
