using System;
using System.Collections.Generic;
using System.Text;
using Application.Tags.Commnads.CreateTag;
using MediatR;

namespace Application.Tags.Queries.GetAllTags
{
    public record class GetAllTagQuery
    (
    ) : IRequest<List<GetAllTagResponse>>;

    public record GetAllTagResponse(
        Guid Id,
        string Title,
        string Description,
        string Slug
    );
}
