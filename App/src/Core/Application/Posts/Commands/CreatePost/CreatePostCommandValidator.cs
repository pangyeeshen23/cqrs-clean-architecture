using Application.Tags.Commnads.CreateTag;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(25).WithMessage("Title must not exceed 25 characters.");

            RuleFor(x => x.Content)
               .NotEmpty().WithMessage("Content is required.");
        }
    }
}
