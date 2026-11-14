using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class PartnerRepository(APIContext context) : GenericRepository<Partner>(context), IPartnerRepository
    {
    }
}
