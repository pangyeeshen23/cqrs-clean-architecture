using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Posts.Queries.GetAllPosts
{
    public record GetAllPostQuery
    (
    ) : IRequest<List<GetAllPostResponse>>;

    public record GetAllPostResponse(
        Guid Id,
        string Title,
        string Content,
        IEnumerable<TagResponse> Tags
    );

    public record TagResponse(
        Guid Id,
        string Title
    );
}
