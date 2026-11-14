using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class SeatTypeRepository(APIContext context) : GenericRepository<SeatType>(context), ISeatTypeRepository
    {
    }
}
