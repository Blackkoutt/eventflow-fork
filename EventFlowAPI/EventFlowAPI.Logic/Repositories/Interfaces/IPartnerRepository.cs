using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface IPartnerRepository : IEntityRepository<Partner>
    {
        Task<IReadOnlyList<Partner>> GetPageAsync(PartnerQuery query);
        /// <summary>All partners of a kind, soft-deleted ones included.</summary>
        Task<IReadOnlyList<Partner>> GetByKindAsync(PartnerKind kind);
        Task<IReadOnlyList<Partner>> GetActiveByIdsAsync(PartnerKind kind, IEnumerable<int> ids);
        Task<bool> ActiveNameExistsAsync(PartnerKind? kind, string name, int? excludedId = null);
        /// <summary>True when any festival has this partner, so it can only be soft-deleted.</summary>
        Task<bool> IsUsedAsync(int id);
        /// <summary>For each partner of the kind that has any of the given festivals: its name and how many of them it has.</summary>
        Task<IReadOnlyList<(string Name, int FestivalCount)>> CountFestivalsAsync(PartnerKind kind, IEnumerable<int> festivalIds);
    }
}
