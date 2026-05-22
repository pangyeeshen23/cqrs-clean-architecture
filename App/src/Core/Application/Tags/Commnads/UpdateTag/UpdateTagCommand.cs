using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.UpdateTag
{
    public record UpdateTagCommand(
        Guid Id,
        string Title,
        string Description
    ) : IRequest;
}
