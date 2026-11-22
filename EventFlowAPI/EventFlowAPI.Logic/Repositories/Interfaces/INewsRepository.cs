using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface INewsRepository : IEntityRepository<News>
    {
        Task<IReadOnlyList<News>> GetPageAsync(NewsQuery query);
        Task<bool> TitleExistsAsync(string title, int? excludedId = null);
    }
}
