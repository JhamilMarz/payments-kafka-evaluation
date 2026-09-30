# BNB — API de Registro de Pagos de Servicios Básicos

Minimal API en .NET 8 para registrar y consultar pagos de servicios básicos
(agua, electricidad, telecomunicaciones).

## Reglas de negocio

- Estado inicial del pago: **pendiente**
- Monto máximo aceptado: **1500 Bs**
- Solo se aceptan montos en **bolivianos (BOB)**. Se rechazan dólares.

## Stack

- .NET 8 (Minimal API)
- Entity Framework Core + SQL Server
- FluentValidation
- Swagger
- Docker / docker-compose

## Estructura

- `Endpoints/` → definición de rutas
- `Services/` → lógica de negocio
- `DTOs/` → contratos de entrada/salida
- `Validators/` → reglas de validación
- `Data/` → DbContext
- `Middleware/` → manejo global de errores

## Ejecución local

1. Ajusta la cadena de conexión en `appsettings.json`.
2. `dotnet restore`
3. `dotnet run`
4. Swagger: `https://localhost:5001/swagger`

## Ejecución con Docker

```bash
docker compose up --build
```
