using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query.Abstract;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email);
        Task<User?> GetOneAsync(string id);
        Task<IEnumerable<User>> GetAllAsync(QueryObject? query = null);
        Task AddAsync(User user);
        void Update(User user);
    }
}
