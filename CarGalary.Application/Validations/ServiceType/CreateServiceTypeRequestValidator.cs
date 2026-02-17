using CarGalary.Application.Dtos.ServiceType.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.ServiceType
{
    public class CreateServiceTypeRequestValidator : AbstractValidator<CreateServiceTypeRequestDto>
    {
        public CreateServiceTypeRequestValidator(){RuleFor(x=>x.NameAr).NotEmpty(); RuleFor(x=>x.NameEn).NotEmpty();}
    }
}
