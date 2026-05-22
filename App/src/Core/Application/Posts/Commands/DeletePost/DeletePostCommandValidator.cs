using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Posts.Commands.DeletePost
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator() 
        { 
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Post Id is required")
                .Must(id => id != Guid.Empty).WithMessage("Post Id must be a valid GUID");
        }
    }
}
