using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarGalary.Application.Dtos;
using CarGalary.Application.Dtos.Auth;
using FluentValidation;

namespace arGalary.Application.Validations.CarFeature
{
   public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.UserName) || !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("UserName or Email is required");

            RuleFor(x => x.UserName)
                .MinimumLength(3)
                .WithMessage("UserName must be at least 3 characters long")
                .When(x => !string.IsNullOrWhiteSpace(x.UserName));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email is not valid")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
                

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}
