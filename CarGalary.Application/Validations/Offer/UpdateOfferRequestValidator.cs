using CarGalary.Application.Dtos.Offer.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Offer
{
    public class UpdateOfferRequestValidator : AbstractValidator<UpdateOfferRequestDto>
    {
        public UpdateOfferRequestValidator(){RuleFor(x=>x.OfferNameAr).NotEmpty(); RuleFor(x=>x.OfferNameEn).NotEmpty();}
    }
}
