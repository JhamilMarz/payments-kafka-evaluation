namespace PaymentApi.Models;

public class Payment
{
    public Guid PaymentId { get; set; } = Guid.NewGuid();
    public string CustomerId { get; set; } = default!;
    public string ServiceProvider { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BOB";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pendiente;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}