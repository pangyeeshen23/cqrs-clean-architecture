using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(25).WithMessage("Title must not exceed 25 characters.");
        }
    }
}
