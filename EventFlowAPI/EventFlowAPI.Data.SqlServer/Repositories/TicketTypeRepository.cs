using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class TicketTypeRepository(APIContext context) : GenericRepository<TicketType>(context), ITicketTypeRepository
    {
    }
}
