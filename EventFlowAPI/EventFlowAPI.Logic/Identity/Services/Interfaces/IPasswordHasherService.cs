using EventFlowAPI.DB.Entities;

namespace EventFlowAPI.Logic.Identity.Services.Interfaces
{
    public interface IPasswordHasherService
    {
        string Hash(User user, string password);
        bool Verify(User user, string password);
    }
}
