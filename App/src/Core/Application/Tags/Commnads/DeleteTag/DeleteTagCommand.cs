using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.DeleteTag
{
    public record DeleteTagCommand
    (
        Guid Id
    ) : IRequest;

}
