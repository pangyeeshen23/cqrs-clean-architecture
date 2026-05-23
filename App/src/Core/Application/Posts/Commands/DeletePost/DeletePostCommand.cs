using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Posts.Commands.DeletePost
{
    public record DeletePostCommand(
        Guid Id
    ) : IRequest;
}
