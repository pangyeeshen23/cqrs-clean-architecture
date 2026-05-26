using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Users.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        string FullName,
        int? Age,
        string? PhoneNumber
    ) : IRequest<UpdateUserProfileResponse>;

    public record UpdateUserProfileResponse
    (
    );
}
