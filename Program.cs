using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PaymentApi.Data;
using PaymentApi.Endpoints;
using PaymentApi.Middleware;
using PaymentApi.Services;
using PaymentApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// BD
builder.Services.AddDbContext<PaymentsDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Validadores
builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentRequestValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware global de errores
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Crea la BD si no existe (en prod usar migraciones)
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
    db.Database.EnsureCreated();
}

app.MapPaymentEndpoints();

app.Run();