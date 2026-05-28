using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Test.Core.Seeder
{
    public class UserSeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        public static Guid Id { get; } = Guid.Parse("4f9a3e2b-7c8d-4f6e-82a1-9c3b4e5f6d7a");
        public static string Username { get; } = "ethanpang";
        public static string Email { get; } = "ethanpang@gmail.com";
        public static string FullName { get; } = "Pang Yee Shen";
        public static int Age { get; } = 23;
        public static string PhoneNumber { get; } = "60122792350";


        public UserSeeder(IUserRepository repository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> SeedUser(Guid? id = null)
        {
            User user = new User
            {
                Id = id ?? Id,
                Username = Username,
                Email = Email,
                IsActive = true,
                Profile = new UserProfile
                {
                    FullName = FullName,
                    Age = Age,
                    PhoneNumber = PhoneNumber
                }
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, "!root123Qwe123");
            await _userRepository.AddAsync(user);
            return user;
        }
    }
}
