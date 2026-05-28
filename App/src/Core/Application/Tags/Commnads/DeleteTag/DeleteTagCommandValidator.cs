using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Tags.Commnads.DeleteTag
{
    public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tag Id is required.")
                .Must(id => id != Guid.Empty).WithMessage("Tag Id must be a valid GUID.");
        }
    }
}
