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
        }
    }
}
