using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface IFAQRepository : IEntityRepository<FAQ>
    {
        Task<IReadOnlyList<FAQ>> GetPageAsync(FAQQuery query);
        Task<bool> QuestionExistsAsync(string question, int? excludedId = null);
    }
}
