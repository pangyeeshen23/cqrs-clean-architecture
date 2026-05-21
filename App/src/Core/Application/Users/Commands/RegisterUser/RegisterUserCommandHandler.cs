using Domain.Authentication;
using Domain.Caching;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

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
            User? user = await _userRepository.GetByEmailAsync(request.Email);
            if (user != null)
            {
                throw new Exception("User with this email already exists.");
            }
            user = new User
            {
                Name = $"{request.Name}",
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(null!, request.Password),
                IsActive = true,
                Profile = new UserProfile
                {
                    Age = request.Age,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address
                }
            };
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, "User");
            var cacheKey = $"user:{user.Id}";
            await _cacheService.SetAsync(cacheKey, new { user.Id, user.Email, user.Name }, TimeSpan.FromHours(1), cancellationToken);
            await _userRepository.AddAsync(user);
            return new RegisterUserResponse(user.Id, user.Email, user.Name, token);
        }
    }
}
