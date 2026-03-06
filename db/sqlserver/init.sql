/**
 * @Author: Carlos Galeano
 * @Date:   2026-03-04 18:10:58
 * @Last Modified by:   Carlos Galeano
 * @Last Modified time: 2026-03-04 18:18:49
 */
-- Ajusta 'AppDb' si usas otro nombre en .env
IF DB_ID('AppDb') IS NULL
BEGIN
  EXEC('CREATE DATABASE [AppDb]');
END
GO

USE [AppDb];
GO

IF OBJECT_ID('dbo.Todos', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Todos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    IsDone BIT NOT NULL DEFAULT(0),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
  );
END
GO

INSERT INTO dbo.Todos (Title, IsDone) VALUES (N'Primer TODO en SQL Server', 0);
GO