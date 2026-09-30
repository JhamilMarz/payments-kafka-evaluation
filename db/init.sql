/* =========================================================
   Payments DB: esquema + seeds
   Se ejecuta UNA sola vez al inicializar el contenedor.
   Es idempotente: se puede re-ejecutar sin duplicar datos.
   ========================================================= */

IF DB_ID('PaymentsDb') IS NULL
BEGIN
    CREATE DATABASE PaymentsDb;
END
GO

USE PaymentsDb;
GO

/* ---------- Payments ---------- */
IF OBJECT_ID('dbo.Payments', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Payments
    (
        PaymentId       UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Payments PRIMARY KEY DEFAULT NEWID(),
        CustomerId      NVARCHAR(100)    NOT NULL,
        ServiceProvider NVARCHAR(200)    NOT NULL,
        Amount          DECIMAL(18,2)    NOT NULL,
        Currency        NVARCHAR(3)      NOT NULL CONSTRAINT DF_Payments_Currency DEFAULT 'BOB',
        Status          NVARCHAR(20)     NOT NULL CONSTRAINT DF_Payments_Status   DEFAULT 'Pendiente',
        CreatedAt       DATETIME2        NOT NULL CONSTRAINT DF_Payments_Created  DEFAULT SYSUTCDATETIME(),
        CONSTRAINT CK_Payments_Amount   CHECK (Amount > 0 AND Amount <= 1500),
        CONSTRAINT CK_Payments_Currency CHECK (Currency = 'BOB'),
        CONSTRAINT CK_Payments_Status   CHECK (Status IN ('Pendiente','Aprobado','Rechazado'))
    );

    CREATE INDEX IX_Payments_CustomerId ON dbo.Payments(CustomerId);
END
GO

/* ---------- OutboxMessages ---------- */
IF OBJECT_ID('dbo.OutboxMessages', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.OutboxMessages
    (
        Id           UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_OutboxMessages PRIMARY KEY DEFAULT NEWID(),
        EventType    NVARCHAR(100)    NOT NULL,
        Payload      NVARCHAR(MAX)    NOT NULL,
        Status       NVARCHAR(20)     NOT NULL CONSTRAINT DF_Outbox_Status DEFAULT 'Pending',
        RetryCount   INT              NOT NULL CONSTRAINT DF_Outbox_Retry  DEFAULT 0,
        LastError    NVARCHAR(1000)   NULL,
        CreatedAt    DATETIME2        NOT NULL CONSTRAINT DF_Outbox_Created DEFAULT SYSUTCDATETIME(),
        PublishedAt  DATETIME2        NULL,
        CONSTRAINT CK_Outbox_Status CHECK (Status IN ('Pending','Published','Failed'))
    );

    CREATE INDEX IX_Outbox_Status_CreatedAt ON dbo.OutboxMessages(Status, CreatedAt);
END
GO

/* ---------- Seeds ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Payments)
BEGIN
    DECLARE @cust UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';

    INSERT INTO dbo.Payments (PaymentId, CustomerId, ServiceProvider, Amount, Currency, Status, CreatedAt)
    VALUES
      (NEWID(), @cust, 'SERVICIOS ELECTRICOS S.A.',        120.50, 'BOB', 'Pendiente', DATEADD(DAY,-3, SYSUTCDATETIME())),
      (NEWID(), @cust, 'EPSAS - AGUA POTABLE',              85.20, 'BOB', 'Aprobado',  DATEADD(DAY,-2, SYSUTCDATETIME())),
      (NEWID(), @cust, 'ENTEL TELECOMUNICACIONES',         199.99, 'BOB', 'Pendiente', DATEADD(DAY,-1, SYSUTCDATETIME())),
      (NEWID(), '22222222-2222-2222-2222-222222222222', 'CRE - ELECTRICIDAD', 320.00, 'BOB', 'Pendiente', SYSUTCDATETIME());
END
GO