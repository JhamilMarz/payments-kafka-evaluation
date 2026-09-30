using Microsoft.EntityFrameworkCore;
using PaymentApi.Data;
using PaymentApi.DTOs;
using PaymentApi.Models;

namespace PaymentApi.Services;

public class PaymentService : IPaymentService
{
    private readonly PaymentsDbContext _db;

    public PaymentService(PaymentsDbContext db) => _db = db;

    public async Task<PaymentResponse> CreateAsync(CreatePaymentRequest request, CancellationToken ct = default)
    {
        var payment = new Payment
        {
            CustomerId = request.CustomerId,
            ServiceProvider = request.ServiceProvider,
            Amount = request.Amount,
            Currency = (request.Currency ?? "BOB").ToUpperInvariant(),
            Status = PaymentStatus.Pendiente,
            CreatedAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync(ct);

        return ToResponse(payment);
    }

    public async Task<IReadOnlyList<PaymentResponse>> GetByCustomerAsync(string customerId, CancellationToken ct = default)
    {
        return await _db.Payments
            .AsNoTracking()
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PaymentResponse(
                p.PaymentId,
                p.ServiceProvider,
                p.Amount,
                p.Status.ToString().ToLowerInvariant(),
                p.CreatedAt))
            .ToListAsync(ct);
    }

    private static PaymentResponse ToResponse(Payment p) =>
        new(p.PaymentId,
            p.ServiceProvider,
            p.Amount,
            p.Status.ToString().ToLowerInvariant(),
            p.CreatedAt);
}