using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query.Abstract;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Logic.Repositories.Repositories
{
    public sealed class UserRepository(APIContext context) : IUserRepository
    {
        private readonly APIContext _context = context;

        public async Task<User?> FindByEmailAsync(string email)
        {
            var lowerCaseEmail = email.ToLower();

            return await _context.Users
                        .Include(u => u.UserData)
                        .FirstOrDefaultAsync(u => u.Email.ToLower() == lowerCaseEmail);
        }

        public async Task<User?> GetOneAsync(string id)
        {
            return await _context.Users
                        .Include(u => u.UserData)
                        .Include(u => u.Reservations)
                        .Include(u => u.EventPasses)
                        .Include(u => u.HallRents)
                        .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync(QueryObject? query = null)
        {
            IQueryable<User> users = _context.Users.Include(u => u.UserData);

            if (query != null)
            {
                users = users.SortBy(query.SortBy, query.SortDirection)
                             .GetPage(query.PageNumber, query.PageSize);
            }

            return await users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}
