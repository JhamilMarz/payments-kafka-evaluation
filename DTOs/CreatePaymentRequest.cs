namespace PaymentApi.DTOs;

public record CreatePaymentRequest(
    string CustomerId,
    string ServiceProvider,
    decimal Amount,
    string? Currency = "BOB"
);