using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Posts.Commands.UpdatePost
{
    public record UpdatePostCommand(
        Guid Id,
        string Title,
        string Content,
        List<Guid> TagIds
    ) : IRequest<UpdatePostResponse>;

    public record UpdatePostResponse(
        Guid id
    );
}
