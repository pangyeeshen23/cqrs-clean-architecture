using System;
using System.Collections.Generic;
using System.Text;
using Application.Users.Commands.LoginUser;
using Domain.Authentication;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Model.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.GenerateUser
{
    public class GenerateUserCommandHandler : IRequestHandler<GenerateUserCommand, GenerateUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<GenerateUserCommandHandler> _logger;
        public GenerateUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ILogger<GenerateUserCommandHandler> logger
        )
        {
            _userRepository = userRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }
        public async Task<GenerateUserResponse> Handle(GenerateUserCommand request, CancellationToken cancellationToken)
        {
            for (int i = 0; i < request.Count; i++)
            {
                User user = new User
                {
                    Username = $"demo+-" + i,
                    Email = $"demo+-" + i + "@gmail.com",
                    IsActive = true,
                    Profile = new UserProfile
                    {
                        FullName = "Demo Name",
                        Age = i,
                        PhoneNumber = "60122792350"
                    }
                };
                user.PasswordHash = _passwordHasher.HashPassword(user, "!root123Qwe123");
                await _userRepository.AddAsync(user);
                _logger.LogInformation("Hello," + i);
            }
            return new GenerateUserResponse(true);
        }
    }
}
