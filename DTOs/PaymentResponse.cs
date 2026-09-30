namespace PaymentApi.DTOs;

public record PaymentResponse(
    Guid PaymentId,
    string ServiceProvider,
    decimal Amount,
    string Status,
    DateTime CreatedAt
);