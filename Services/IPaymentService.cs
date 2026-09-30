using PaymentApi.DTOs;

namespace PaymentApi.Services;

public interface IPaymentService
{
    Task<PaymentResponse> CreateAsync(CreatePaymentRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<PaymentResponse>> GetByCustomerAsync(string customerId, CancellationToken ct = default);
}