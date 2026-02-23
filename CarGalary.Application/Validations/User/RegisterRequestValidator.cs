
using CarGalary.Application.Dtos;
using CarGalary.Application.Dtos.Auth;
using FluentValidation;


namespace arGalary.Application.Validations.CarFeature
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            // RuleFor(x => x.Username)
            //     .NotEmpty().WithMessage("Username is required")
            //     .Length(3, 50).WithMessage("Username must be between 3 and 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

                 RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required")
                .MinimumLength(3).WithMessage("UserName must be at least 3 characters long");;
               

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("Branch is required");

            // RuleFor(x => x.Roles)
            //     .NotEmpty().WithMessage("At least one role must be selected")
            //     .Must(roles => roles.All(r => !string.IsNullOrWhiteSpace(r)))
            //     .WithMessage("Roles cannot contain empty values");
        }
    }
}