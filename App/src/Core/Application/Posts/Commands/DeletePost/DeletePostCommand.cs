using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Posts.Commands.DeletePost
{
    public record DeletePostCommand(
        Guid Id
    ) : IRequest;
}
