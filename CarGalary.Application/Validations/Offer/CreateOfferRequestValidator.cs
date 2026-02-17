using CarGalary.Application.Dtos.Offer.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Offer
{
    public class CreateOfferRequestValidator : AbstractValidator<CreateOfferRequestDto>
    {
        public CreateOfferRequestValidator(){RuleFor(x=>x.OfferNameAr).NotEmpty(); RuleFor(x=>x.OfferNameEn).NotEmpty();}
    }
}
