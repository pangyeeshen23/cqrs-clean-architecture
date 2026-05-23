using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Posts.Commands.CreatePost
{
    public record CreatePostCommand(
        string Title,
        string Content,
        List<Guid> Tags
    ) : IRequest<CreatePostResponse>;

    public record CreatePostResponse(
        Guid id
    );
}
