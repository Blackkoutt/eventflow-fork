using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query.Abstract;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Logic.Repositories.Repositories
{
    public sealed class UserRepository(APIContext context) : GenericRepository<User>(context), IUserRepository
    {
        public async Task<User?> FindByEmailAsync(string email)
        {
            var lowerCaseEmail = email.ToLower();

            return await _context.Users
                        .FirstOrDefaultAsync(u => u.Email.ToLower() == lowerCaseEmail);
        }

        public async Task<User?> GetOneAsync(string id)
        {
            return await _context.Users
                        .Include(u => u.Reservations)
                        .Include(u => u.EventPasses)
                        .Include(u => u.HallRents)
                        .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync(QueryObject? query = null)
        {
            IQueryable<User> users = _context.Users;

            if (query != null)
            {
                users = users.SortBy(query.SortBy, query.SortDirection)
                             .GetPage(query.PageNumber, query.PageSize);
            }

            return await users.ToListAsync();
        }
    }
}
