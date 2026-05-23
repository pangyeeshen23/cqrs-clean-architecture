using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Users.Queries.GetUserProfile
{
    public record GetUserProfileQuery
    (
        Guid UserId
    ) : IRequest<GetUserProfileResponse>;

    public record GetUserProfileResponse(
        string Username,
        string Email,
        string FullName,
        int Age,
        string PhoneNumber,
        string Address
    );
}
