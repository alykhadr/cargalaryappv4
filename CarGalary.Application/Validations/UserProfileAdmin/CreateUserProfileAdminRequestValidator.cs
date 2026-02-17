using CarGalary.Application.Dtos.UserProfileAdmin.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.UserProfileAdmin
{
    public class CreateUserProfileAdminRequestValidator : AbstractValidator<CreateUserProfileAdminRequestDto>
    {
        public CreateUserProfileAdminRequestValidator(){RuleFor(x=>x.UserId).NotEmpty().WithMessage("UserId is required");}
    }
}
