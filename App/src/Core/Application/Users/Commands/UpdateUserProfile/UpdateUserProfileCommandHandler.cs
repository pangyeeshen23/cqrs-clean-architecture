using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Users;
using Domain.Repositories;
using Domain.Repositories.Model.UserProfiles;
using Domain.Repositories.Model.Users;
using MediatR;

namespace Application.Users.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResponse>
    {

        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ICurrentUserService _currentUser;
        public UpdateUserProfileCommandHandler(
            IUserProfileRepository userProfileRepository,
            ICurrentUserService currentUser
        )
        {
            _userProfileRepository = userProfileRepository;
            _currentUser = currentUser;
        }


        public async Task<UpdateUserProfileResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            UserProfileFilterModel filter = new UserProfileFilterModel();
            filter.UserId = _currentUser.UserId;
            UserProfile userProfile = await _userProfileRepository.GetAsync(filter) ?? throw new NotFoundException("User");
            if (request.FullName != null) userProfile.FullName = request.FullName;
            if (request.Age != null) userProfile.Age = request.Age.Value;
            if (request.PhoneNumber != null) userProfile.PhoneNumber = request.PhoneNumber;
            if (request.Address != null) userProfile.Address = request.Address;
            await _userProfileRepository.UpdateAsync(userProfile);
            return new UpdateUserProfileResponse();
        }

    }
}
