using CarGalary.Application.Dtos.Car.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Car
{
    public class CreateCarRequestValidator : AbstractValidator<CreateCarRequestDto>
    {
        public CreateCarRequestValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().WithMessage("NameAr is required");
            RuleFor(x => x.NameEn).NotEmpty().WithMessage("NameEn is required");
            RuleFor(x => x.ModelId).GreaterThan(0).WithMessage("ModelId is required");
            RuleFor(x => x.TypeId).GreaterThan(0).WithMessage("TypeId is required");
            RuleFor(x => x.BranchId).GreaterThan(0).WithMessage("BranchId is required");
            RuleFor(x => x.Year).GreaterThan(1900).WithMessage("Year is required");
            RuleFor(x => x.Mileage).GreaterThanOrEqualTo(0).WithMessage("Mileage must be zero or greater");
            RuleFor(x => x.Vat).NotNull().GreaterThanOrEqualTo(0).WithMessage("Vat is required");
            RuleFor(x => x.ConditionId).NotNull().GreaterThan(0).WithMessage("ConditionId is required");
            RuleFor(x => x.SeatingCapacity).NotNull().GreaterThan(0).WithMessage("SeatingCapacity is required");
            RuleFor(x => x.WeelSizeInch).NotEmpty().WithMessage("WeelSizeInch is required");
            RuleFor(x => x.FuelTankCapacityLiter).NotNull().GreaterThan(0).WithMessage("FuelTankCapacityLiter is required");
            RuleFor(x => x.TrimLevel).NotNull().GreaterThan(0).WithMessage("TrimLevel is required");
            RuleFor(x => x.VehicleClass).NotNull().GreaterThan(0).WithMessage("VehicleClass is required");
            RuleFor(x => x.PlateNumber).NotEmpty().WithMessage("PlateNumber is required");
            RuleFor(x => x.TransmisionType).NotNull().GreaterThan(0).WithMessage("TransmisionType is required");
            RuleFor(x => x.Drivetrain).NotNull().GreaterThan(0).WithMessage("Drivetrain is required");
            RuleFor(x => x.Cylenders).NotNull().GreaterThan(0).WithMessage("Cylenders is required");
            RuleFor(x => x.FuelType).NotNull().GreaterThan(0).WithMessage("FuelType is required");
            RuleFor(x => x.EnginNumber).NotEmpty().WithMessage("EnginNumber is required");
            RuleFor(x => x.DescriptionAr).NotEmpty().WithMessage("DescriptionAr is required");
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage("DescriptionEn is required");
        }
    }
}
