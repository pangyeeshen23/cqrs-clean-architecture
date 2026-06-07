using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Users.Queries.GetUserList
{
    public record GetUserListQuery
    (

    ) : IRequest<IEnumerable<GetUserListResponse>>;

    public record GetUserListResponse(
        string Username,
        string Email
    );
}
