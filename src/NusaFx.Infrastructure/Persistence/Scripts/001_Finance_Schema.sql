-- ============================================================
-- Schema: Finance
-- Purpose: Currency conversion and transaction logging
-- ============================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'NusaFxDb')
BEGIN
    CREATE DATABASE NusaFxDb;
END
GO

USE NusaFxDb;
GO

IF OBJECT_ID(N'[Transaction]', N'U') IS NOT NULL
    DROP TABLE [Transaction];
GO

CREATE TABLE [Transaction]
(
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Transaction PRIMARY KEY
        DEFAULT NEWID(),

    Description NVARCHAR(255) NOT NULL,

    Amount DECIMAL(18,2) NOT NULL
        CONSTRAINT CHK_Transaction_Amount_Positive CHECK (Amount > 0),

    Currency CHAR(3) NOT NULL
        CONSTRAINT CHK_Transaction_Currency_Allowed CHECK (Currency IN ('USD','MYR')),

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Transaction_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO


-- ConversionLog table
IF OBJECT_ID(N'ConversionLog', N'U') IS NOT NULL
    DROP TABLE ConversionLog;
GO

CREATE TABLE ConversionLog
(
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_ConversionLog PRIMARY KEY
        DEFAULT NEWID(),

    FromCurrency CHAR(3) NOT NULL,
    ToCurrency   CHAR(3) NOT NULL,

    Amount DECIMAL(18,2) NOT NULL
        CONSTRAINT CHK_ConversionLog_Amount_Positive CHECK (Amount >= 0),

    LiveRate DECIMAL(18,6) NOT NULL,
    LiveResult DECIMAL(18,2) NOT NULL,

    AiResult DECIMAL(18,2) NULL,
    Difference DECIMAL(18,2) NULL,

    ConvertedAt DATETIME2 NOT NULL
        CONSTRAINT DF_ConversionLog_ConvertedAt DEFAULT SYSUTCDATETIME()
);
GO


-- ============================================================
-- Indexes
-- ============================================================

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Transaction_CreatedAt')
    CREATE INDEX IX_Transaction_CreatedAt ON [Transaction](CreatedAt);

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ConversionLog_ConvertedAt')
    CREATE INDEX IX_ConversionLog_ConvertedAt ON ConversionLog(ConvertedAt);

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ConversionLog_CurrencyPair')
    CREATE INDEX IX_ConversionLog_CurrencyPair ON ConversionLog(FromCurrency, ToCurrency, ConvertedAt);
GO

