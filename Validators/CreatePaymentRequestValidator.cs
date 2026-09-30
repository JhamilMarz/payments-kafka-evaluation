using FluentValidation;
using PaymentApi.DTOs;

namespace PaymentApi.Validators;

public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    private const decimal MaxAmountBs = 1500m;

    public CreatePaymentRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("customerId es requerido.")
            .Must(BeGuid).WithMessage("customerId debe ser un GUID válido.");

        RuleFor(x => x.ServiceProvider)
            .NotEmpty().WithMessage("serviceProvider es requerido.")
            .MaximumLength(200);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.")
            .LessThanOrEqualTo(MaxAmountBs)
            .WithMessage($"El monto no puede superar {MaxAmountBs} Bs.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(c => string.Equals(c, "BOB", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Solo se aceptan pagos en Bolivianos (BOB). No se aceptan dólares.");
    }

    private static bool BeGuid(string value) => Guid.TryParse(value, out _);
}