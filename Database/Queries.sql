-- =====================================================================
-- PATHOLAB SYSTEM - DROP & SELECT STATEMENTS SCRIPT
-- Database: RWD
-- All tables are prefixed with PTH_ and sorted in proper dependency order.
-- =====================================================================

USE [RWD];
GO

-- =====================================================================
-- SECTION 1: DROP TABLES STATEMENTS (Safely dropping constraints first)
-- =====================================================================
PRINT '--- DROPPING ALL FOREIGN KEY CONSTRAINTS ---';
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
    + N'.' + QUOTENAME(OBJECT_NAME(parent_object_id)) 
    + N' DROP CONSTRAINT ' + QUOTENAME(name) + N';' + CHAR(13)
FROM sys.foreign_keys;
EXEC sp_executesql @sql;
GO

PRINT '--- DROPPING ALL PTH_ TABLES (IF EXIST) ---';

IF OBJECT_ID('dbo.PTH_Notifications', 'U') IS NOT NULL DROP TABLE [PTH_Notifications];
IF OBJECT_ID('dbo.PTH_AuditLogs', 'U') IS NOT NULL DROP TABLE [PTH_AuditLogs];
IF OBJECT_ID('dbo.PTH_HomeCollections', 'U') IS NOT NULL DROP TABLE [PTH_HomeCollections];
IF OBJECT_ID('dbo.PTH_ReportDetails', 'U') IS NOT NULL DROP TABLE [PTH_ReportDetails];
IF OBJECT_ID('dbo.PTH_Reports', 'U') IS NOT NULL DROP TABLE [PTH_Reports];
IF OBJECT_ID('dbo.PTH_Payments', 'U') IS NOT NULL DROP TABLE [PTH_Payments];
IF OBJECT_ID('dbo.PTH_Invoices', 'U') IS NOT NULL DROP TABLE [PTH_Invoices];
IF OBJECT_ID('dbo.PTH_TestResultHistory', 'U') IS NOT NULL DROP TABLE [PTH_TestResultHistory];
IF OBJECT_ID('dbo.PTH_TestResults', 'U') IS NOT NULL DROP TABLE [PTH_TestResults];
IF OBJECT_ID('dbo.PTH_SampleTests', 'U') IS NOT NULL DROP TABLE [PTH_SampleTests];
IF OBJECT_ID('dbo.PTH_Samples', 'U') IS NOT NULL DROP TABLE [PTH_Samples];
IF OBJECT_ID('dbo.PTH_OrderDetails', 'U') IS NOT NULL DROP TABLE [PTH_OrderDetails];
IF OBJECT_ID('dbo.PTH_Orders', 'U') IS NOT NULL DROP TABLE [PTH_Orders];
IF OBJECT_ID('dbo.PTH_PackageTests', 'U') IS NOT NULL DROP TABLE [PTH_PackageTests];
IF OBJECT_ID('dbo.PTH_TestPackages', 'U') IS NOT NULL DROP TABLE [PTH_TestPackages];
IF OBJECT_ID('dbo.PTH_TestParameters', 'U') IS NOT NULL DROP TABLE [PTH_TestParameters];
IF OBJECT_ID('dbo.PTH_Tests', 'U') IS NOT NULL DROP TABLE [PTH_Tests];
IF OBJECT_ID('dbo.PTH_SampleTypes', 'U') IS NOT NULL DROP TABLE [PTH_SampleTypes];
IF OBJECT_ID('dbo.PTH_Departments', 'U') IS NOT NULL DROP TABLE [PTH_Departments];
IF OBJECT_ID('dbo.PTH_Doctors', 'U') IS NOT NULL DROP TABLE [PTH_Doctors];
IF OBJECT_ID('dbo.PTH_Patients', 'U') IS NOT NULL DROP TABLE [PTH_Patients];
IF OBJECT_ID('dbo.PTH_RolePermissions', 'U') IS NOT NULL DROP TABLE [PTH_RolePermissions];
IF OBJECT_ID('dbo.PTH_Permissions', 'U') IS NOT NULL DROP TABLE [PTH_Permissions];
IF OBJECT_ID('dbo.PTH_Users', 'U') IS NOT NULL DROP TABLE [PTH_Users];
IF OBJECT_ID('dbo.PTH_Roles', 'U') IS NOT NULL DROP TABLE [PTH_Roles];

PRINT '--- ALL PTH_ TABLES DROPPED ---';
GO

-- =====================================================================
-- SECTION 2: SELECT STATEMENTS FOR ALL 25 TABLES
-- =====================================================================
PRINT '--- QUERYING TABLE RECORDS ---';

SELECT * FROM [dbo].[PTH_Roles];
SELECT * FROM [dbo].[PTH_Users];
SELECT * FROM [dbo].[PTH_Permissions];
SELECT * FROM [dbo].[PTH_RolePermissions];
SELECT * FROM [dbo].[PTH_Patients];
SELECT * FROM [dbo].[PTH_Doctors];
SELECT * FROM [dbo].[PTH_Departments];
SELECT * FROM [dbo].[PTH_SampleTypes];
SELECT * FROM [dbo].[PTH_Tests];
SELECT * FROM [dbo].[PTH_TestParameters];
SELECT * FROM [dbo].[PTH_TestPackages];
SELECT * FROM [dbo].[PTH_PackageTests];
SELECT * FROM [dbo].[PTH_Orders];
SELECT * FROM [dbo].[PTH_OrderDetails];
SELECT * FROM [dbo].[PTH_Samples];
SELECT * FROM [dbo].[PTH_SampleTests];
SELECT * FROM [dbo].[PTH_TestResults];
SELECT * FROM [dbo].[PTH_TestResultHistory];
SELECT * FROM [dbo].[PTH_Invoices];
SELECT * FROM [dbo].[PTH_Payments];
SELECT * FROM [dbo].[PTH_Reports];
SELECT * FROM [dbo].[PTH_ReportDetails];
SELECT * FROM [dbo].[PTH_HomeCollections];
SELECT * FROM [dbo].[PTH_AuditLogs];
SELECT * FROM [dbo].[PTH_Notifications];
GO
