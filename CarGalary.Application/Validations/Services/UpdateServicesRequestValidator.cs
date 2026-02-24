using CarGalary.Application.Dtos.Services.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Services
{
    public class UpdateServicesRequestValidator : AbstractValidator<UpdateServicesRequestDto>
    {
        public UpdateServicesRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DescriptionAr).NotEmpty();
            RuleFor(x => x.DescriptionEn).NotEmpty();
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);
        }
    }
}
