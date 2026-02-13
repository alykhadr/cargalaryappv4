
using CarGalary.Application.Dtos;
using FluentValidation;

public class AssignCarFeaturesDtoValidator 
    : AbstractValidator<AssignCarFeaturesDto>
{
    public AssignCarFeaturesDtoValidator()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("CarId must be greater than zero");

        RuleFor(x => x.FeatureIds)
            .NotEmpty()
            .WithMessage("At least one feature must be selected");

        RuleForEach(x => x.FeatureIds)
            .GreaterThan(0)
            .WithMessage("FeatureId must be greater than zero");
    }
}
