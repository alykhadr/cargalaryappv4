using CarGalary.Application.Dtos.Offer.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Offer
{
    public class CreateOfferRequestValidator : AbstractValidator<CreateOfferRequestDto>
    {
        public CreateOfferRequestValidator()
        {
            RuleFor(x => x.OfferNameAr).NotEmpty().WithMessage("OfferNameAr is required");
            RuleFor(x => x.OfferNameEn).NotEmpty().WithMessage("OfferNameEn is required");
            RuleFor(x => x.ImageFile).NotNull().WithMessage("Image is required");
            RuleFor(x => x.ExpiredAt).NotNull().WithMessage("Expiry date is required");
        }
    }
}
