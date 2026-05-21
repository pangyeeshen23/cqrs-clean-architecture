using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher<User>
    {
        private readonly IPasswordHasher<object> _passwordHasher;

        public PasswordHasher()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(new object(), password);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(new object(), hashedPassword, providedPassword);
            return result;
        }
    }
}
