using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Test.Core.Seeder
{
    public class UserSeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserSeeder(IUserRepository repository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> SeedUser()
        {
            User user = new User
            {
                Username = "ethanpang",
                Email = "ethanpang@gmail.com",
                IsActive = true,
                Profile = new UserProfile
                {
                    FullName = "Pang Yee Shen",
                    Age = 23,
                    PhoneNumber = "60122792350"
                }
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, "!root123Qwe123");
            await _userRepository.AddAsync(user);
            return user;
        }
    }
}
