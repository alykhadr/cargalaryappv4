using CarGalary.Application.Dtos.Offer.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Offer
{
    public class UpdateOfferRequestValidator : AbstractValidator<UpdateOfferRequestDto>
    {
        public UpdateOfferRequestValidator()
        {
            RuleFor(x => x.OfferNameAr).NotEmpty().WithMessage("OfferNameAr is required");
            RuleFor(x => x.OfferNameEn).NotEmpty().WithMessage("OfferNameEn is required");
            RuleFor(x => x.ExpiredAt).NotNull().WithMessage("Expiry date is required");

            When(x => x.IsPercentage, () =>
            {
                RuleFor(x => x.PercentageValue)
                    .NotNull().WithMessage("PercentageValue is required when IsPercentage is true")
                    .InclusiveBetween(0.01m, 100m).WithMessage("PercentageValue must be between 0.01 and 100");
            });

            When(x => !x.IsPercentage, () =>
            {
                RuleFor(x => x.PercentageValue)
                    .Must(value => value == null || value == 0)
                    .WithMessage("PercentageValue is only applicable when IsPercentage is true");
            });
        }
    }
}
