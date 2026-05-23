using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .Must(id => id != Guid.Empty).WithMessage("Post Id must be a valid Guid");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(5).WithMessage("Title must not be less than 5 characters.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Content)
               .NotEmpty().WithMessage("Content is required.");
        }
    }
}
