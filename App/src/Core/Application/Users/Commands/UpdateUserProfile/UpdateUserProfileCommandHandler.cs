using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResponse>
    {

        private readonly IUserRepository userRepository;
        public UpdateUserProfileCommandHandler()
        {
            
        }


        public Task<UpdateUserProfileResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
