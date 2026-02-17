using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.UserFavoriteAdmin
{
    public class UpdateUserFavoriteAdminRequestValidator : AbstractValidator<UpdateUserFavoriteAdminRequestDto>
    {
        public UpdateUserFavoriteAdminRequestValidator(){RuleFor(x=>x.Priority).GreaterThanOrEqualTo(0);} 
    }
}
