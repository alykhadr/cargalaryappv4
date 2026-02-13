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
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required")
                .MinimumLength(3).WithMessage("UserName must be at least 3 characters long");
                

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}