using FluentValidation;
using PaymentApi.DTOs;
using PaymentApi.Services;

namespace PaymentApi.Endpoints;

public static class PaymentEndpoints
{
    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/payments")
                       .WithTags("Payments");

        // POST /api/payments
        group.MapPost("/", async (
            CreatePaymentRequest request,
            IValidator<CreatePaymentRequest> validator,
            IPaymentService service,
            CancellationToken ct) =>
        {
            var result = await validator.ValidateAsync(request, ct);
            if (!result.IsValid)
                return Results.ValidationProblem(result.ToDictionary());

            var created = await service.CreateAsync(request, ct);
            return Results.Created($"/api/payments/{created.PaymentId}", created);
        })
        .WithName("CreatePayment")
        .Produces<PaymentResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .WithSummary("Registra un pago de servicio básico.")
        .WithDescription("Solo acepta montos en BOB <= 1500. El estado inicial es 'pendiente'.");

        // GET /api/payments?customerId=...
        group.MapGet("/", async (
            string customerId,
            IPaymentService service,
            CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return Results.BadRequest(new { error = "El parámetro customerId es obligatorio." });

            var payments = await service.GetByCustomerAsync(customerId, ct);
            return Results.Ok(payments);
        })
        .WithName("GetPaymentsByCustomer")
        .Produces<IEnumerable<PaymentResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Consulta los pagos de un cliente por customerId.");

        return app;
    }
}