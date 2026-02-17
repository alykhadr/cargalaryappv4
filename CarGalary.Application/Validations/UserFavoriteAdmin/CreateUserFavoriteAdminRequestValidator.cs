using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.UserFavoriteAdmin
{
    public class CreateUserFavoriteAdminRequestValidator : AbstractValidator<CreateUserFavoriteAdminRequestDto>
    {
        public CreateUserFavoriteAdminRequestValidator(){RuleFor(x=>x.UserId).NotEmpty(); RuleFor(x=>x.CarId).GreaterThan(0);} 
    }
}
