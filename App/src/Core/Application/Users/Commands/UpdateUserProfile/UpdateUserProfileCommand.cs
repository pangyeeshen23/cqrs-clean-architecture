using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        int? Age,
        string? PhoneNumber,
        string? Address
    ) : IRequest<UpdateUserProfileResponse>;

    public record UpdateUserProfileResponse
    (
        bool Success
    );
}
