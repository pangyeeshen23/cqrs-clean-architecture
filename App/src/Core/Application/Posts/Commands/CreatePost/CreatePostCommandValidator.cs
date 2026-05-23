using System;
using System.Collections.Generic;
using System.Text;
using Application.Tags.Commnads.CreateTag;
using FluentValidation;

namespace Application.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(5).WithMessage("Title must not be less than 5 characters.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Content)
               .NotEmpty().WithMessage("Content is required.");
        }
    }
}
