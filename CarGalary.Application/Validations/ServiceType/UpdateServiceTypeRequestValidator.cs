using CarGalary.Application.Dtos.ServiceType.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.ServiceType
{
    public class UpdateServiceTypeRequestValidator : AbstractValidator<UpdateServiceTypeRequestDto>
    {
        public UpdateServiceTypeRequestValidator(){RuleFor(x=>x.NameAr).NotEmpty(); RuleFor(x=>x.NameEn).NotEmpty();}
    }
}
