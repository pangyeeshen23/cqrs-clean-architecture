using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Tags.Commnads.DeleteTag
{
    public record DeleteTagCommand
    (
        Guid Id
    ) : IRequest<DeleteTagResponse>;

    public record DeleteTagResponse(
        bool result
    );
}
