using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Users.Commands.GenerateUser
{
    public record GenerateUserCommand(
        int Count = 100
    ) : IRequest<GenerateUserResponse>;

    public record GenerateUserResponse(
        bool Generated
    );
}
