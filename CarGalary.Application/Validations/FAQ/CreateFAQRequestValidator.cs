using CarGalary.Application.Dtos.FAQ.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.FAQ
{
    public class CreateFAQRequestValidator : AbstractValidator<CreateFAQRequestDto>
    {
        public CreateFAQRequestValidator()
        {
            RuleFor(x => x.TitleAr).NotEmpty().WithMessage("TitleAr is required");
            RuleFor(x => x.TitleEn).NotEmpty().WithMessage("TitleEn is required");
            RuleFor(x => x.DescriptionAr).NotEmpty().WithMessage("DescriptionAr is required");
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage("DescriptionEn is required");
            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
        }
    }
}
