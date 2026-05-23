using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email format");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Username must contain only letters and numbers");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name must contain only letters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(12).WithMessage("Password length must be at least 12 characters")
                .MaximumLength(20).WithMessage("Password length must not exceed 20 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$")
                .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Confirm Passwords do not match with Password");

            RuleFor(x => x.Age)
                .GreaterThan(1).WithMessage("Age must be greater than 1")
                .LessThanOrEqualTo(100).WithMessage("Age must be less than or equal to 100");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?(\d{1,3})?[-.\s]?(\(?\d{1,4}\)?[-.\s]?){1,3}\d{1,4}[-.\s]?\d{1,9}$")
                .WithMessage("Invalid phone number format");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MinimumLength(10).WithMessage("Address must be at least 10 characters long");
        }
    }
}
