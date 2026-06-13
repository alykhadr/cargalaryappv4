using CarGalary.Application.Dtos.Invoice.Command;
using FluentValidation;

namespace CarGalary.Application.Validations.Invoice
{
    public class UpdateInvoiceRequestValidator : AbstractValidator<UpdateInvoiceRequestDto>
    {
        public UpdateInvoiceRequestValidator()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("BranchId is required.");

            RuleFor(x => x.PaymentMethod)
                .GreaterThan(0).WithMessage("PaymentMethod is required.");

            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("InvoiceNumber is required.")
                .MaximumLength(50);

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("CustomerName is required.")
                .MaximumLength(200);

            RuleFor(x => x.CustomerPhone)
                .NotEmpty().WithMessage("CustomerPhone is required.")
                .MaximumLength(20);

            RuleFor(x => x.CustomerEmail)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.CustomerEmail))
                .MaximumLength(256);

            RuleFor(x => x.CustomerAddress)
                .MaximumLength(500);

            RuleFor(x => x.Notes)
                .MaximumLength(1000);

            RuleFor(x => x.ShippingFee)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ExtraDiscount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("At least one invoice detail is required.");

            RuleForEach(x => x.Details).SetValidator(new UpdateInvoiceDetailRequestValidator());
        }
    }

    public class UpdateInvoiceDetailRequestValidator : AbstractValidator<UpdateInvoiceDetailRequestDto>
    {
        public UpdateInvoiceDetailRequestValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("CarId is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity is required.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be greater than or equal to 0.");

            RuleFor(x => x.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage("DiscountAmount must be greater than or equal to 0.");

            RuleFor(x => x.VatAmount)
                .GreaterThanOrEqualTo(0).WithMessage("VatAmount must be greater than or equal to 0.");

            RuleFor(x => x.Notes)
                .MaximumLength(300);
        }
    }
}
