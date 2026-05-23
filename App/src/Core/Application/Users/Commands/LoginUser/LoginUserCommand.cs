using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Users.Commands.LoginUser
{
    public record LoginUserCommand(
        string Username,
        string Password
    ) : IRequest<LoginUserResponse>;

    public record LoginUserResponse(
        string Token
    );
}
