using CarGalary.Application.Dtos.Auth;
using FluentValidation;

namespace CarGalary.Application.Validations.User
{
    public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role name is required")
                .MaximumLength(64).WithMessage("Role name cannot exceed 64 characters");
        }
    }
}
