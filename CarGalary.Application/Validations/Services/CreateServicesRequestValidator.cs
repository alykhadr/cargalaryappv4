using CarGalary.Application.Dtos.Services.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Services
{
    public class CreateServicesRequestValidator : AbstractValidator<CreateServicesRequestDto>
    {
        public CreateServicesRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DescriptionAr).NotEmpty();
            RuleFor(x => x.DescriptionEn).NotEmpty();
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ImageFile).NotNull().WithMessage("Image is required");
        }
    }
}
