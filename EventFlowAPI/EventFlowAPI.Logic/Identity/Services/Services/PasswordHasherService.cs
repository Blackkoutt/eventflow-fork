using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EventFlowAPI.Logic.Identity.Services.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        public string Hash(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool Verify(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result != PasswordVerificationResult.Failed;
        }
    }
}
