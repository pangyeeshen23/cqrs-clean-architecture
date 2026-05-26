using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Users.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
        {

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters")
                .Matches(@"^[a-zA-Z]+$").WithMessage("Name must contain only letters");

            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Age is required")
                .GreaterThan(1).WithMessage("Age must be greater than 1")
                .LessThanOrEqualTo(100).WithMessage("Age must be less than or equal to 100");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?(\d{1,3})?[-.\s]?(\(?\d{1,4}\)?[-.\s]?){1,3}\d{1,4}[-.\s]?\d{1,9}$")
                .WithMessage("Invalid phone number format");

        }
    }
}
