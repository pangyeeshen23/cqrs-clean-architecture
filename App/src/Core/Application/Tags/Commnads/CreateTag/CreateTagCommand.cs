using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Tags.Commnads.CreateTag
{
    public record CreateTagCommand(
        string Title,
        string Description
    ) : IRequest<CreateTagResponse>;

    public record CreateTagResponse(
        Guid Id
    );
}
