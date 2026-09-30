using Microsoft.EntityFrameworkCore;
using PaymentApi.Models;

namespace PaymentApi.Data;

public class PaymentsDbContext : DbContext
{
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options) { }

    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var p = modelBuilder.Entity<Payment>();
        p.ToTable("Payments");
        p.HasKey(x => x.PaymentId);
        p.Property(x => x.CustomerId).IsRequired().HasMaxLength(100);
        p.Property(x => x.ServiceProvider).IsRequired().HasMaxLength(200);
        p.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        p.Property(x => x.Currency).IsRequired().HasMaxLength(3);
        p.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        p.Property(x => x.CreatedAt).IsRequired();
        p.HasIndex(x => x.CustomerId);
    }
}