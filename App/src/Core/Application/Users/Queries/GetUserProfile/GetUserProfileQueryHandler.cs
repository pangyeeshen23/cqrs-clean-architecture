using System.Security.Claims;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Users;
using Domain.Repositories;
using Domain.Repositories.Model.Users;
using MediatR;

namespace Application.Users.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, GetUserProfileResponse>
    {
        private readonly IUserRepository _userRepository;
        public GetUserProfileQueryHandler(
            IUserRepository userRepository
        )
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            UserFilterModel filter = new UserFilterModel();
            filter.Id = request.UserId;
            filter.IsIncludeUserProfile = true;
            User user = await _userRepository.GetByAsync(filter) ?? throw new NotFoundException("User");
            return new GetUserProfileResponse(user.Username, user.Email, user.Profile.FullName, user.Profile.Age, user.Profile.PhoneNumber);
        }
    }
}
