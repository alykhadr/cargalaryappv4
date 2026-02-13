using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarGalary.Application.Dtos;
using FluentValidation;

namespace arGalary.Application.Validations.CarFeature
{
   public class AddFavoriteDtoValidator : AbstractValidator<AddFavoriteDto>
{
    public AddFavoriteDtoValidator()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("CarId must be greater than zero");
    }
}
}