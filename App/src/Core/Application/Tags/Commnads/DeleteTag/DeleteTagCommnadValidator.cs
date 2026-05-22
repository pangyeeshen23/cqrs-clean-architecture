using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.DeleteTag
{
    public class DeleteTagCommnadValidator : AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommnadValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tag Id is required.")
                .Must(id => id != Guid.Empty).WithMessage("Tag Id must be a valid GUID.");   
        }
    }
}
