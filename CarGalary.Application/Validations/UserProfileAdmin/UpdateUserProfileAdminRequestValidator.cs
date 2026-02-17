using CarGalary.Application.Dtos.UserProfileAdmin.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.UserProfileAdmin
{
    public class UpdateUserProfileAdminRequestValidator : AbstractValidator<UpdateUserProfileAdminRequestDto>
    {
        public UpdateUserProfileAdminRequestValidator(){RuleFor(x=>x.UserId).NotEmpty().WithMessage("UserId is required");}
    }
}
