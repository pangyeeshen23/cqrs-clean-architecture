using Domain.Authentication;
using Domain.Caching;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Users;
using Domain.Repositories;
using Domain.Repositories.Model.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.LoginUser
{
    public class LoginUserCommnadHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICacheService _cacheService;
        public LoginUserCommnadHandler(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            ICacheService cacheService
        )
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _cacheService = cacheService;
            _passwordHasher = passwordHasher;
        }
        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            UserFilterModel filterModel = new UserFilterModel();
            filterModel.Username = request.Username;
            User? user = await _userRepository.GetByAsync(filterModel);
            if(user == null) throw new NotFoundException("User");
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed) throw new InvalidCredentialException();
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, "User");
            var cacheKey = $"user:{user.Id}";
            await _cacheService.SetAsync(cacheKey, new { user.Id, user.Email, user.Username }, TimeSpan.FromHours(1), cancellationToken);
            return new LoginUserResponse(token);
        }
    }
}
