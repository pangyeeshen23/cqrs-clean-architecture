using Domain.Authentication;
using Domain.Caching;
using Domain.Entities;
using Domain.Exceptions.Users;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICacheService _cacheService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            ICacheService cacheService,
            IPasswordHasher<User> passwordHasher
        )
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _cacheService = cacheService;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetByAsync(request.Username, request.Email);
            if (user != null) throw new UserAlreadyExistsException("User with this username/email already exists.");
            user = new User
            {
                Username = $"{request.Username}",
                Email = request.Email,
                IsActive = true,
                Profile = new UserProfile
                {
                    FullName = request.Username,
                    Age = request.Age,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address
                }
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, "User");
            var cacheKey = $"user:{user.Id}";
            await _cacheService.SetAsync(cacheKey, new { user.Id, user.Email, user.Username }, TimeSpan.FromHours(1), cancellationToken);
            await _userRepository.AddAsync(user);
            return new RegisterUserResponse(token);
        }
    }
}
