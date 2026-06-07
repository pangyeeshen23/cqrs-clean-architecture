using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Users.Commands.GenerateUser
{
    public class GenerateUserCommandValidator : AbstractValidator<GenerateUserCommand>
    {
        public GenerateUserCommandValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(100).WithMessage("Count must be greater than 100");
        }
    }
}
