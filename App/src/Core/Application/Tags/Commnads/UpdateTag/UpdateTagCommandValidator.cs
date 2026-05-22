using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.UpdateTag
{
    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tag Id is required.")
                .Must(id => id != Guid.Empty).WithMessage("Tag Id must be a valid GUID.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(25).WithMessage("Title must not exceed 25 characters");
        }
    }
}
