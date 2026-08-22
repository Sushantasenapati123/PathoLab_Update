-- =====================================================================
-- PATHOLAB DIAGNOSTIC MANAGEMENT SYSTEM DATABASE DEPLOYMENT SCRIPT
-- Target DBMS: Microsoft SQL Server (2016 or higher)
-- Database Name: RWD
-- Creates all schemas and populates master & fictional demo cycles.
-- Note: All tables are prefixed with PTH_ as requested.
-- =====================================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RWD')
BEGIN
    CREATE DATABASE [RWD];
END
GO

USE [RWD];
GO

-- Drop existing tables (Safely dropping all foreign keys first to prevent constraint violations)
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
GO

-- =====================================================================
-- SECTION 1: SCHEMAS CREATION WITH PTH_ PREFIX (DDL)
-- =====================================================================

CREATE TABLE [PTH_Roles] (
    [RoleId] INT IDENTITY(1,1) NOT NULL,
    [RoleName] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(250) NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_Roles_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_Roles_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_Roles_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Roles_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

CREATE TABLE [PTH_Users] (
    [UserId] INT IDENTITY(1,1) NOT NULL,
    [Username] NVARCHAR(50) NOT NULL,
    [PasswordHash] NVARCHAR(256) NOT NULL,
    [FullName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Mobile] NVARCHAR(15) NOT NULL,
    [RoleId] INT NOT NULL,
    [LastLoginDate] DATETIME2 NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_Users_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_Users_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_Users_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Users_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Users] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_PTH_Users_PTH_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [PTH_Roles] ([RoleId])
);
CREATE UNIQUE NONCLUSTERED INDEX [IX_PTH_Users_Username] ON [PTH_Users] ([Username] ASC) WHERE [DeletedFlag] = 0;

CREATE TABLE [PTH_Permissions] (
    [PermissionId] INT IDENTITY(1,1) NOT NULL,
    [PermissionCode] NVARCHAR(50) NOT NULL,
    [PermissionName] NVARCHAR(100) NOT NULL,
    [ModuleName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(250) NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_Permissions_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_Permissions_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_Permissions_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Permissions_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Permissions] PRIMARY KEY CLUSTERED ([PermissionId] ASC)
);
CREATE UNIQUE NONCLUSTERED INDEX [IX_PTH_Permissions_PermissionCode] ON [PTH_Permissions] ([PermissionCode] ASC);

CREATE TABLE [PTH_RolePermissions] (
    [RolePermissionId] INT IDENTITY(1,1) NOT NULL,
    [RoleId] INT NOT NULL,
    [PermissionId] INT NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_RolePermissions_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_RolePermissions_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_RolePermissions_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_RolePermissions] PRIMARY KEY CLUSTERED ([RolePermissionId] ASC),
    CONSTRAINT [FK_PTH_RolePermissions_PTH_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [PTH_Roles] ([RoleId]),
    CONSTRAINT [FK_PTH_RolePermissions_PTH_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [PTH_Permissions] ([PermissionId])
);
CREATE UNIQUE NONCLUSTERED INDEX [IX_PTH_RolePermissions_RoleId_PermissionId] ON [PTH_RolePermissions] ([RoleId] ASC, [PermissionId] ASC);

CREATE TABLE [PTH_Patients] (
    [PatientId] INT IDENTITY(1,1) NOT NULL,
    [PatientCode] NVARCHAR(20) NOT NULL,
    [FirstName] NVARCHAR(50) NOT NULL,
    [MiddleName] NVARCHAR(50) NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [Gender] NVARCHAR(20) NOT NULL,
    [DateOfBirth] DATETIME2 NOT NULL,
    [Age] INT NOT NULL,
    [BloodGroup] NVARCHAR(10) NULL,
    [Mobile] NVARCHAR(20) NOT NULL,
    [AlternateMobile] NVARCHAR(15) NULL,
    [Email] NVARCHAR(100) NULL,
    [Address] NVARCHAR(250) NULL,
    [City] NVARCHAR(50) NULL,
    [State] NVARCHAR(50) NULL,
    [Pincode] NVARCHAR(10) NULL,
    [EmergencyContactName] NVARCHAR(100) NULL,
    [EmergencyContactMobile] NVARCHAR(15) NULL,
    [IdentityType] NVARCHAR(50) NULL,
    [IdentityNumber] NVARCHAR(50) NULL,
    [Remarks] NVARCHAR(500) NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_Patients_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_Patients_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_Patients_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Patients_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Patients] PRIMARY KEY CLUSTERED ([PatientId] ASC)
);
CREATE UNIQUE NONCLUSTERED INDEX [IX_PTH_Patients_PatientCode] ON [PTH_Patients] ([PatientCode] ASC) WHERE [DeletedFlag] = 0;

CREATE TABLE [PTH_Doctors] (
    [DoctorId] INT IDENTITY(1,1) NOT NULL,
    [DoctorCode] NVARCHAR(20) NOT NULL,
    [DoctorName] NVARCHAR(150) NOT NULL,
    [Qualification] NVARCHAR(100) NULL,
    [Specialization] NVARCHAR(100) NULL,
    [HospitalClinicName] NVARCHAR(200) NULL,
    [Mobile] NVARCHAR(15) NOT NULL,
    [Email] NVARCHAR(100) NULL,
    [Address] NVARCHAR(250) NULL,
    [CommissionType] NVARCHAR(20) NOT NULL,
    [CommissionValue] DECIMAL(18,2) NOT NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_Doctors_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_Doctors_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_Doctors_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Doctors_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Doctors] PRIMARY KEY CLUSTERED ([DoctorId] ASC)
);
CREATE UNIQUE NONCLUSTERED INDEX [IX_PTH_Doctors_DoctorCode] ON [PTH_Doctors] ([DoctorCode] ASC) WHERE [DeletedFlag] = 0;

CREATE TABLE [PTH_Departments] (
    [DepartmentId] INT IDENTITY(1,1) NOT NULL,
    [DepartmentCode] NVARCHAR(20) NOT NULL,
    [DepartmentName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(250) NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_Departments_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_Departments_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_Departments_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Departments_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Departments] PRIMARY KEY CLUSTERED ([DepartmentId] ASC)
);

CREATE TABLE [PTH_SampleTypes] (
    [SampleTypeId] INT IDENTITY(1,1) NOT NULL,
    [SampleTypeCode] NVARCHAR(20) NOT NULL,
    [SampleTypeName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(250) NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_SampleTypes_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_SampleTypes_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_SampleTypes_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_SampleTypes_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_SampleTypes] PRIMARY KEY CLUSTERED ([SampleTypeId] ASC)
);

CREATE TABLE [PTH_Tests] (
    [TestId] INT IDENTITY(1,1) NOT NULL,
    [TestCode] NVARCHAR(20) NOT NULL,
    [TestName] NVARCHAR(150) NOT NULL,
    [DepartmentId] INT NOT NULL,
    [SampleTypeId] INT NOT NULL,
    [TestType] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [TATMinutes] INT NOT NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_Tests_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_Tests_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_Tests_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Tests_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Tests] PRIMARY KEY CLUSTERED ([TestId] ASC),
    CONSTRAINT [FK_PTH_Tests_PTH_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [PTH_Departments] ([DepartmentId]),
    CONSTRAINT [FK_PTH_Tests_PTH_SampleTypes_SampleTypeId] FOREIGN KEY ([SampleTypeId]) REFERENCES [PTH_SampleTypes] ([SampleTypeId])
);

CREATE TABLE [PTH_TestParameters] (
    [TestParameterId] INT IDENTITY(1,1) NOT NULL,
    [TestId] INT NOT NULL,
    [ParameterCode] NVARCHAR(50) NOT NULL,
    [ParameterName] NVARCHAR(150) NOT NULL,
    [DisplayOrder] INT NOT NULL,
    [ResultType] NVARCHAR(50) NOT NULL,
    [Unit] NVARCHAR(50) NULL,
    [MaleMin] DECIMAL(18,4) NULL,
    [MaleMax] DECIMAL(18,4) NULL,
    [FemaleMin] DECIMAL(18,4) NULL,
    [FemaleMax] DECIMAL(18,4) NULL,
    [ChildMin] DECIMAL(18,4) NULL,
    [ChildMax] DECIMAL(18,4) NULL,
    [CriticalLow] DECIMAL(18,4) NULL,
    [CriticalHigh] DECIMAL(18,4) NULL,
    [DefaultReferenceRange] NVARCHAR(250) NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_TestParameters_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_TestParameters_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_TestParameters_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_TestParameters] PRIMARY KEY CLUSTERED ([TestParameterId] ASC),
    CONSTRAINT [FK_PTH_TestParameters_PTH_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [PTH_Tests] ([TestId]) ON DELETE CASCADE
);

CREATE TABLE [PTH_TestPackages] (
    [PackageId] INT IDENTITY(1,1) NOT NULL,
    [PackageCode] NVARCHAR(30) NOT NULL,
    [PackageName] NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [OriginalPrice] DECIMAL(18,2) NOT NULL,
    [PackagePrice] DECIMAL(18,2) NOT NULL,
    [DiscountAmount] DECIMAL(18,2) NOT NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_PTH_TestPackages_IsActive] DEFAULT (1),
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_TestPackages_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_TestPackages_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_TestPackages_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_TestPackages] PRIMARY KEY CLUSTERED ([PackageId] ASC)
);

CREATE TABLE [PTH_PackageTests] (
    [PackageTestId] INT IDENTITY(1,1) NOT NULL,
    [PackageId] INT NOT NULL,
    [TestId] INT NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_PackageTests_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_PackageTests_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_PackageTests_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_PackageTests] PRIMARY KEY CLUSTERED ([PackageTestId] ASC),
    CONSTRAINT [FK_PTH_PackageTests_PTH_TestPackages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [PTH_TestPackages] ([PackageId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PTH_PackageTests_PTH_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [PTH_Tests] ([TestId])
);

CREATE TABLE [PTH_Orders] (
    [OrderId] INT IDENTITY(1,1) NOT NULL,
    [OrderNumber] NVARCHAR(30) NOT NULL,
    [PatientId] INT NOT NULL,
    [DoctorId] INT NULL,
    [OrderDate] DATETIME2 NOT NULL,
    [OrderStatus] NVARCHAR(30) NOT NULL,
    [TotalAmount] DECIMAL(18,2) NOT NULL,
    [DiscountAmount] DECIMAL(18,2) NOT NULL,
    [NetAmount] DECIMAL(18,2) NOT NULL,
    [PaidAmount] DECIMAL(18,2) NOT NULL,
    [DueAmount] DECIMAL(18,2) NOT NULL,
    [PaymentStatus] NVARCHAR(20) NOT NULL,
    [Remarks] NVARCHAR(500) NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Orders_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Orders] PRIMARY KEY CLUSTERED ([OrderId] ASC),
    CONSTRAINT [FK_PTH_Orders_PTH_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [PTH_Patients] ([PatientId])
);

CREATE TABLE [PTH_OrderDetails] (
    [OrderDetailId] INT IDENTITY(1,1) NOT NULL,
    [OrderId] INT NOT NULL,
    [TestId] INT NULL,
    [PackageId] INT NULL,
    [Quantity] INT NOT NULL,
    [Rate] DECIMAL(18,2) NOT NULL,
    [Discount] DECIMAL(18,2) NOT NULL,
    [Amount] DECIMAL(18,2) NOT NULL,
    [SampleRequired] BIT NOT NULL,
    [Status] NVARCHAR(30) NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_OrderDetails_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_OrderDetails_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_OrderDetails_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_OrderDetails] PRIMARY KEY CLUSTERED ([OrderDetailId] ASC),
    CONSTRAINT [FK_PTH_OrderDetails_PTH_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [PTH_Orders] ([OrderId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PTH_OrderDetails_PTH_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [PTH_Tests] ([TestId]),
    CONSTRAINT [FK_PTH_OrderDetails_PTH_TestPackages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [PTH_TestPackages] ([PackageId])
);

CREATE TABLE [PTH_Samples] (
    [SampleId] INT IDENTITY(1,1) NOT NULL,
    [SampleNumber] NVARCHAR(30) NOT NULL,
    [OrderId] INT NOT NULL,
    [PatientId] INT NOT NULL,
    [SampleTypeId] INT NOT NULL,
    [Barcode] NVARCHAR(50) NOT NULL,
    [CollectionDateTime] DATETIME2 NULL,
    [CollectedById] INT NULL,
    [ReceivedDateTime] DATETIME2 NULL,
    [ReceivedById] INT NULL,
    [SampleStatus] NVARCHAR(20) NOT NULL,
    [RejectionReason] NVARCHAR(250) NULL,
    [Remarks] NVARCHAR(250) NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Samples_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Samples] PRIMARY KEY CLUSTERED ([SampleId] ASC),
    CONSTRAINT [FK_PTH_Samples_PTH_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [PTH_Orders] ([OrderId]),
    CONSTRAINT [FK_PTH_Samples_PTH_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [PTH_Patients] ([PatientId]),
    CONSTRAINT [FK_PTH_Samples_PTH_SampleTypes_SampleTypeId] FOREIGN KEY ([SampleTypeId]) REFERENCES [PTH_SampleTypes] ([SampleTypeId]),
    CONSTRAINT [FK_PTH_Samples_PTH_Users_CollectedById] FOREIGN KEY ([CollectedById]) REFERENCES [PTH_Users] ([UserId]),
    CONSTRAINT [FK_PTH_Samples_PTH_Users_ReceivedById] FOREIGN KEY ([ReceivedById]) REFERENCES [PTH_Users] ([UserId])
);

CREATE TABLE [PTH_SampleTests] (
    [SampleTestId] INT IDENTITY(1,1) NOT NULL,
    [SampleId] INT NOT NULL,
    [OrderDetailId] INT NOT NULL,
    [TestId] INT NOT NULL,
    [Status] NVARCHAR(30) NOT NULL,
    [AssignedToId] INT NULL,
    [StartedOn] DATETIME2 NULL,
    [CompletedOn] DATETIME2 NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_SampleTests_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_SampleTests_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_SampleTests_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_SampleTests] PRIMARY KEY CLUSTERED ([SampleTestId] ASC),
    CONSTRAINT [FK_PTH_SampleTests_PTH_Samples_SampleId] FOREIGN KEY ([SampleId]) REFERENCES [PTH_Samples] ([SampleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PTH_SampleTests_PTH_OrderDetails_OrderDetailId] FOREIGN KEY ([OrderDetailId]) REFERENCES [PTH_OrderDetails] ([OrderDetailId]),
    CONSTRAINT [FK_PTH_SampleTests_PTH_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [PTH_Tests] ([TestId]),
    CONSTRAINT [FK_PTH_SampleTests_PTH_Users_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [PTH_Users] ([UserId])
);

CREATE TABLE [PTH_TestResults] (
    [TestResultId] INT IDENTITY(1,1) NOT NULL,
    [SampleTestId] INT NOT NULL,
    [TestParameterId] INT NOT NULL,
    [ResultValue] NVARCHAR(200) NULL,
    [ResultNumericValue] DECIMAL(18,4) NULL,
    [ResultStatus] NVARCHAR(20) NOT NULL,
    [ReferenceRange] NVARCHAR(200) NULL,
    [Unit] NVARCHAR(50) NULL,
    [Remarks] NVARCHAR(500) NULL,
    [EnteredById] INT NULL,
    [EnteredOn] DATETIME2 NULL,
    [VerifiedById] INT NULL,
    [VerifiedOn] DATETIME2 NULL,
    [RowVersion] ROWVERSION NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_TestResults_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_TestResults_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_TestResults_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_TestResults] PRIMARY KEY CLUSTERED ([TestResultId] ASC),
    CONSTRAINT [FK_PTH_TestResults_PTH_SampleTests_SampleTestId] FOREIGN KEY ([SampleTestId]) REFERENCES [PTH_SampleTests] ([SampleTestId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PTH_TestResults_PTH_TestParameters_TestParameterId] FOREIGN KEY ([TestParameterId]) REFERENCES [PTH_TestParameters] ([TestParameterId]),
    CONSTRAINT [FK_PTH_TestResults_PTH_Users_EnteredById] FOREIGN KEY ([EnteredById]) REFERENCES [PTH_Users] ([UserId]),
    CONSTRAINT [FK_PTH_TestResults_PTH_Users_VerifiedById] FOREIGN KEY ([VerifiedById]) REFERENCES [PTH_Users] ([UserId])
);

CREATE TABLE [PTH_TestResultHistory] (
    [HistoryId] INT IDENTITY(1,1) NOT NULL,
    [TestResultId] INT NOT NULL,
    [OldValue] NVARCHAR(200) NULL,
    [NewValue] NVARCHAR(200) NULL,
    [OldStatus] NVARCHAR(20) NULL,
    [NewStatus] NVARCHAR(20) NULL,
    [Reason] NVARCHAR(250) NOT NULL,
    [ChangedById] INT NOT NULL,
    [ChangedOn] DATETIME2 NOT NULL,
    CONSTRAINT [PK_PTH_TestResultHistory] PRIMARY KEY CLUSTERED ([HistoryId] ASC),
    CONSTRAINT [FK_PTH_TestResultHistory_PTH_TestResults_TestResultId] FOREIGN KEY ([TestResultId]) REFERENCES [PTH_TestResults] ([TestResultId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PTH_TestResultHistory_PTH_Users_ChangedById] FOREIGN KEY ([ChangedById]) REFERENCES [PTH_Users] ([UserId])
);

CREATE TABLE [PTH_Invoices] (
    [InvoiceId] INT IDENTITY(1,1) NOT NULL,
    [InvoiceNumber] NVARCHAR(30) NOT NULL,
    [OrderId] INT NOT NULL,
    [PatientId] INT NOT NULL,
    [InvoiceDate] DATETIME2 NOT NULL,
    [GrossAmount] DECIMAL(18,2) NOT NULL,
    [DiscountAmount] DECIMAL(18,2) NOT NULL,
    [TaxAmount] DECIMAL(18,2) NOT NULL,
    [NetAmount] DECIMAL(18,2) NOT NULL,
    [PaidAmount] DECIMAL(18,2) NOT NULL,
    [DueAmount] DECIMAL(18,2) NOT NULL,
    [InvoiceStatus] NVARCHAR(20) NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Invoices_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Invoices] PRIMARY KEY CLUSTERED ([InvoiceId] ASC),
    CONSTRAINT [FK_PTH_Invoices_PTH_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [PTH_Orders] ([OrderId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PTH_Invoices_PTH_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [PTH_Patients] ([PatientId])
);

CREATE TABLE [PTH_Payments] (
    [PaymentId] INT IDENTITY(1,1) NOT NULL,
    [InvoiceId] INT NOT NULL,
    [PaymentNumber] NVARCHAR(30) NOT NULL,
    [PaymentDate] DATETIME2 NOT NULL,
    [Amount] DECIMAL(18,2) NOT NULL,
    [PaymentMode] NVARCHAR(50) NOT NULL,
    [TransactionReference] NVARCHAR(100) NULL,
    [Remarks] NVARCHAR(250) NULL,
    [ReceivedBy] NVARCHAR(100) NOT NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Payments_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Payments] PRIMARY KEY CLUSTERED ([PaymentId] ASC),
    CONSTRAINT [FK_PTH_Payments_PTH_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [PTH_Invoices] ([InvoiceId]) ON DELETE CASCADE
);

CREATE TABLE [PTH_Reports] (
    [ReportId] INT IDENTITY(1,1) NOT NULL,
    [ReportNumber] NVARCHAR(30) NOT NULL,
    [OrderId] INT NOT NULL,
    [PatientId] INT NOT NULL,
    [ReportStatus] NVARCHAR(20) NOT NULL,
    [ReportDate] DATETIME2 NOT NULL,
    [VerifiedById] INT NULL,
    [VerifiedOn] DATETIME2 NULL,
    [PublishedOn] DATETIME2 NULL,
    [PdfFilePath] NVARCHAR(500) NULL,
    [VersionNumber] INT NOT NULL,
    [Remarks] NVARCHAR(500) NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Reports_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Reports] PRIMARY KEY CLUSTERED ([ReportId] ASC),
    CONSTRAINT [FK_PTH_Reports_PTH_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [PTH_Orders] ([OrderId]),
    CONSTRAINT [FK_Reports_PTH_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [PTH_Patients] ([PatientId]),
    CONSTRAINT [FK_Reports_PTH_Users_VerifiedById] FOREIGN KEY ([VerifiedById]) REFERENCES [PTH_Users] ([UserId])
);

CREATE TABLE [PTH_ReportDetails] (
    [ReportDetailId] INT IDENTITY(1,1) NOT NULL,
    [ReportId] INT NOT NULL,
    [TestId] INT NOT NULL,
    [TestName] NVARCHAR(150) NOT NULL,
    [DisplayOrder] INT NOT NULL CONSTRAINT [DF_PTH_ReportDetails_DisplayOrder] DEFAULT (0),
    [Interpretation] NVARCHAR(MAX) NULL,
    [Remarks] NVARCHAR(500) NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_ReportDetails_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_ReportDetails_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_ReportDetails_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_ReportDetails] PRIMARY KEY CLUSTERED ([ReportDetailId] ASC),
    CONSTRAINT [FK_PTH_ReportDetails_PTH_Reports_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [PTH_Reports] ([ReportId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PTH_ReportDetails_PTH_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [PTH_Tests] ([TestId])
);

CREATE TABLE [PTH_HomeCollections] (
    [HomeCollectionId] INT IDENTITY(1,1) NOT NULL,
    [RequestNumber] NVARCHAR(30) NOT NULL,
    [PatientId] INT NOT NULL,
    [OrderId] INT NULL,
    [Address] NVARCHAR(250) NOT NULL,
    [City] NVARCHAR(100) NOT NULL,
    [Pincode] NVARCHAR(10) NOT NULL,
    [RequestedDate] DATETIME2 NOT NULL,
    [RequestedTime] NVARCHAR(100) NOT NULL,
    [AssignedAgentId] INT NULL,
    [Status] NVARCHAR(20) NOT NULL CONSTRAINT [DF_PTH_HomeCollections_Status] DEFAULT ('Requested'),
    [CollectionDateTime] DATETIME2 NULL,
    [Remarks] NVARCHAR(250) NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_HomeCollections_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_HomeCollections] PRIMARY KEY CLUSTERED ([HomeCollectionId] ASC),
    CONSTRAINT [FK_PTH_HomeCollections_PTH_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [PTH_Patients] ([PatientId]),
    CONSTRAINT [FK_PTH_HomeCollections_PTH_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [PTH_Orders] ([OrderId]),
    CONSTRAINT [FK_PTH_HomeCollections_PTH_Users_AssignedAgentId] FOREIGN KEY ([AssignedAgentId]) REFERENCES [PTH_Users] ([UserId])
);

CREATE TABLE [PTH_AuditLogs] (
    [AuditLogId] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NULL,
    [ModuleName] NVARCHAR(50) NOT NULL,
    [Action] NVARCHAR(50) NOT NULL,
    [TableName] NVARCHAR(50) NOT NULL,
    [RecordId] INT NOT NULL,
    [OldValues] NVARCHAR(MAX) NULL,
    [NewValues] NVARCHAR(MAX) NULL,
    [IPAddress] NVARCHAR(50) NULL,
    [UserAgent] NVARCHAR(250) NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_AuditLogs_CreatedOn] DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_PTH_AuditLogs] PRIMARY KEY CLUSTERED ([AuditLogId] ASC),
    CONSTRAINT [FK_PTH_AuditLogs_PTH_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [PTH_Users] ([UserId])
);

CREATE TABLE [PTH_Notifications] (
    [NotificationId] INT IDENTITY(1,1) NOT NULL,
    [PatientId] INT NULL,
    [NotificationType] NVARCHAR(20) NOT NULL,
    [Recipient] NVARCHAR(150) NOT NULL,
    [Subject] NVARCHAR(150) NULL,
    [Message] NVARCHAR(MAX) NOT NULL,
    [ReferenceType] NVARCHAR(50) NULL,
    [ReferenceId] INT NULL,
    [Status] NVARCHAR(20) NOT NULL CONSTRAINT [DF_PTH_Notifications_Status] DEFAULT ('Pending'),
    [SentOn] DATETIME2 NULL,
    [FailureReason] NVARCHAR(500) NULL,
    [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [DF_PTH_Notifications_CreatedOn] DEFAULT (GETUTCDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL CONSTRAINT [DF_PTH_Notifications_CreatedBy] DEFAULT ('System'),
    [UpdatedOn] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [DeletedFlag] BIT NOT NULL CONSTRAINT [DF_PTH_Notifications_DeletedFlag] DEFAULT (0),
    CONSTRAINT [PK_PTH_Notifications] PRIMARY KEY CLUSTERED ([NotificationId] ASC),
    CONSTRAINT [FK_PTH_Notifications_PTH_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [PTH_Patients] ([PatientId])
);
GO

-- =====================================================================
-- SECTION 2: POPULATE MASTER & REFERENCE ENTITIES (DML)
-- =====================================================================

-- 1. Insert Roles
INSERT INTO [PTH_Roles] ([RoleName], [Description]) VALUES
('Super Admin', 'Full control and settings audit access'),
('Lab Admin', 'Test parameters settings configuration access'),
('Receptionist', 'Patients intake billing and cash counters'),
('Lab Technician', 'Vials collect, lab receipt, and results entry'),
('Pathologist', 'Awaiting review sign-off validations'),
('Accountant', 'Outstanding collections settlement audit'),
('Collection Agent', 'Home visits blood samples draw');
GO

-- 2. Insert Permissions
INSERT INTO [PTH_Permissions] ([PermissionCode], [PermissionName], [ModuleName], [Description]) VALUES
('PATIENTS_VIEW', 'View Patients', 'Patient Management', 'View patient directories'),
('PATIENTS_CREATE', 'Create Patient', 'Patient Management', 'Register new profile'),
('PATIENTS_EDIT', 'Edit Patient', 'Patient Management', 'Edit profile items'),
('PATIENTS_DELETE', 'Delete Patient', 'Patient Management', 'Delete profile'),
('ORDERS_VIEW', 'View Orders', 'Order Management', 'View orders list'),
('ORDERS_CREATE', 'Create Order', 'Order Management', 'Book new order case'),
('SAMPLES_COLLECT', 'Collect Sample', 'Laboratory Operations', 'Vials draw details'),
('SAMPLES_RECEIVE', 'Receive Sample', 'Laboratory Operations', 'Vials lab receipts'),
('RESULTS_ENTER', 'Enter Results', 'Laboratory Operations', 'Observed values entry'),
('REPORTS_VERIFY', 'Verify Reports', 'Pathology Sign-off', 'Pathologist review approval'),
('REPORTS_PUBLISH', 'Publish Reports', 'Pathology Sign-off', 'Digital sign release'),
('BILLING_VIEW', 'View Invoices', 'Billing & Finance', 'View invoices ledger'),
('BILLING_PAY', 'Collect Payment', 'Billing & Finance', 'Collect balances receipt'),
('ADMIN_USERS', 'Manage Users', 'System Admin', 'Manage staff login accounts');
GO

-- 3. Map Permissions to Roles (RolePermissions)
-- Super Admin (RoleId = 1) gets all Permissions (1-14)
INSERT INTO [PTH_RolePermissions] ([RoleId], [PermissionId]) VALUES
(1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 6), (1, 7), (1, 8), (1, 9), (1, 10), (1, 11), (1, 12), (1, 13), (1, 14);
-- Lab Admin (RoleId = 2) gets all Permissions (1-14)
INSERT INTO [PTH_RolePermissions] ([RoleId], [PermissionId]) VALUES
(2, 1), (2, 2), (2, 3), (2, 4), (2, 5), (2, 6), (2, 7), (2, 8), (2, 9), (2, 10), (2, 11), (2, 12), (2, 13), (2, 14);
-- Receptionist (RoleId = 3) gets Patient & Billing access
INSERT INTO [PTH_RolePermissions] ([RoleId], [PermissionId]) VALUES
(3, 1), (3, 2), (3, 3), (3, 5), (3, 6), (3, 12), (3, 13);
-- Lab Technician (RoleId = 4) gets Vials and result entry
INSERT INTO [PTH_RolePermissions] ([RoleId], [PermissionId]) VALUES
(4, 1), (4, 5), (4, 7), (4, 8), (4, 9);
-- Pathologist (RoleId = 5) gets validations and sign-off
INSERT INTO [PTH_RolePermissions] ([RoleId], [PermissionId]) VALUES
(5, 1), (5, 5), (5, 9), (5, 10), (5, 11);
GO

-- 4. Insert Users (SHA256 passwords correspond to raw values verified by PasswordHasher)
INSERT INTO [PTH_Users] ([Username], [PasswordHash], [FullName], [Email], [Mobile], [RoleId]) VALUES
('admin', 'e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7', 'System Admin', 'admin@patholab.com', '9876543210', 1),
('reception', '238f1cf33d39690fba3c171984fc1120a1181d236723d337e6a2fdc8d92ae88a', 'Rita Sharma', 'reception@patholab.com', '9876543211', 3),
('technician', '7cd5fea080d2a3f7de469cca064541b414698acd4173e0d4823d03351d12a24e', 'Tushar Roy', 'technician@patholab.com', '9876543212', 4),
('pathologist', '51d38824098a6cc0ed0d552ed38a7174ce0d3f53483ed4e00bb7c8e3b832ed5b', 'Dr. Priya Nair', 'pathologist@patholab.com', '9876543213', 5),
('accountant', '381d8632ee588cb77a06f0da28d173fbc17abd815133ce19c4959f74bc19eeb0', 'Anil Mehta', 'accountant@patholab.com', '9876543214', 6),
('agent', 'c3206ca954a4953c1c1b82b4167bd097deab0e228207e3b3296237e369f39d34', 'Amit Verma', 'agent@patholab.com', '9876543215', 7);
GO

-- 5. Insert Departments
INSERT INTO [PTH_Departments] ([DepartmentCode], [DepartmentName], [Description]) VALUES
('HEM', 'Hematology', 'Blood cell assays'),
('BIO', 'Biochemistry', 'Serology and metabolic assays'),
('CLP', 'Clinical Pathology', 'Fluids and Urine Routine analysis');
GO

-- 6. Insert SampleTypes
INSERT INTO [PTH_SampleTypes] ([SampleTypeCode], [SampleTypeName], [Description]) VALUES
('BLD', 'Whole Blood', 'EDTA Vials (Purple-top)'),
('URN', 'Urine Specimen', 'Sterile Urine Container'),
('SER', 'Serum', 'Clot Activator Vials (Red-top)');
GO

-- 7. Insert Tests
INSERT INTO [PTH_Tests] ([TestCode], [TestName], [DepartmentId], [SampleTypeId], [TestType], [Price], [TATMinutes]) VALUES
('CBC', 'Complete Blood Count', 1, 1, 'Profile', 350.00, 120),
('FBS', 'Blood Sugar Fasting', 2, 1, 'Single', 150.00, 60),
('LFT', 'Liver Function Test', 2, 3, 'Profile', 750.00, 180),
('URN', 'Urine Routine', 3, 2, 'Profile', 200.00, 90);
GO

-- 8. Insert TestParameters
-- CBC (TestId = 1)
INSERT INTO [PTH_TestParameters] ([TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange]) VALUES
(1, 'HB', 'Hemoglobin', 1, 'Numeric', 'g/dL', 13.0, 17.0, 12.0, 15.0, 11.0, 14.0, 7.0, 20.0, '12.0 - 17.0'),
(1, 'WBC', 'WBC Count', 2, 'Numeric', '/µL', 4000.0, 11000.0, 4000.0, 11000.0, 5000.0, 13000.0, 2000.0, 30000.0, '4000 - 11000'),
(1, 'PLT', 'Platelet Count', 3, 'Numeric', 'lakh/µL', 1.5, 4.5, 1.5, 4.5, 1.5, 4.5, 0.5, 10.0, '1.5 - 4.5');

-- FBS (TestId = 2)
INSERT INTO [PTH_TestParameters] ([TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange]) VALUES
(2, 'SUG_F', 'Fasting Sugar', 1, 'Numeric', 'mg/dL', 70.0, 100.0, 70.0, 100.0, 70.0, 100.0, 50.0, 350.0, '70 - 100');

-- LFT (TestId = 3)
INSERT INTO [PTH_TestParameters] ([TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange]) VALUES
(3, 'BIL_T', 'Bilirubin Total', 1, 'Numeric', 'mg/dL', 0.1, 1.2, 0.1, 1.2, 0.1, 1.0, 0.0, 5.0, '0.1 - 1.2'),
(3, 'SGPT', 'SGPT (ALT)', 2, 'Numeric', 'U/L', 5.0, 50.0, 5.0, 35.0, 5.0, 40.0, 0.0, 500.0, '5 - 50');

-- Urine (TestId = 4)
INSERT INTO [PTH_TestParameters] ([TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange]) VALUES
(4, 'URN_COL', 'Color', 1, 'Text', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Pale Yellow'),
(4, 'URN_SUG', 'Sugar', 2, 'Text', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Nil');
GO

-- 9. Insert TestPackages
INSERT INTO [PTH_TestPackages] ([PackageCode], [PackageName], [Description], [OriginalPrice], [PackagePrice], [DiscountAmount]) VALUES
('WEL-CHK', 'General Wellness Package', 'Includes CBC, FBS, and LFT profiles at bundled savings.', 1250.00, 999.00, 251.00);
GO

-- 10. Map Package Tests
INSERT INTO [PTH_PackageTests] ([PackageId], [TestId]) VALUES
(1, 1), -- CBC
(1, 2), -- FBS
(1, 3); -- LFT
GO

-- =====================================================================
-- SECTION 3: POPULATE 5 FULL CLINICAL CASE RECORDS (DML DEMO)
-- =====================================================================

-- 1. Insert 5 Patients
INSERT INTO [PTH_Patients] ([PatientCode], [FirstName], [LastName], [Gender], [DateOfBirth], [Age], [BloodGroup], [Mobile], [Email], [Address], [City], [State], [Pincode]) VALUES
('PAT-000001', 'Aarav', 'Sharma', 'Male', '1990-05-15', 36, 'O+', '9812345601', 'aarav.sharma@gmail.com', 'Flat 101, Galaxy Tower, Sector 15', 'Navi Mumbai', 'Maharashtra', '400703'),
('PAT-000002', 'Ananya', 'Sen', 'Female', '1995-10-22', 30, 'A+', '9812345602', 'ananya.sen@gmail.com', 'Flat 202, Regency Park, Sector 11', 'Navi Mumbai', 'Maharashtra', '400703'),
('PAT-000003', 'Rohan', 'Mehta', 'Male', '1982-08-04', 44, 'B+', '9812345603', 'rohan.mehta@gmail.com', 'Flat 303, Tulip Residency, Sector 20', 'Navi Mumbai', 'Maharashtra', '400703'),
('PAT-000004', 'Sanjana', 'Joshi', 'Female', '1999-01-30', 27, 'O-', '9812345604', 'sanjana.joshi@gmail.com', 'Flat 404, Sunshine CHS, Sector 21', 'Navi Mumbai', 'Maharashtra', '400703'),
('PAT-000005', 'Vijay', 'Singh', 'Male', '1965-03-12', 61, 'AB+', '9812345605', 'vijay.singh@gmail.com', 'Flat 505, Palm Beach View, Sector 4', 'Navi Mumbai', 'Maharashtra', '400703');
GO

-- 2. Insert 2 Doctors
INSERT INTO [PTH_Doctors] ([DoctorCode], [DoctorName], [Qualification], [Specialization], [HospitalClinicName], [Mobile], [Email], [Address], [CommissionType], [CommissionValue]) VALUES
('DOC-000001', 'Dr. Rajesh Nair', 'MBBS, MD', 'General Medicine', 'Apex Multispeciality Care', '9988776601', 'dr.rajesh@apexcare.com', 'Apex Arcade, Suite 10', 'Percentage', 10.00),
('DOC-000002', 'Dr. Priya Desai', 'MBBS, DNB', 'Gynecology', 'Desai Womens Clinic', '9988776602', 'dr.desai@clinic.com', 'Desai Chambers, Suite 2', 'Percentage', 10.00);
GO


-- -------------------------------------------------------------
-- DEMO CASE 1: Patient Aarav Sharma (PatientId = 1) - Published Report
-- -------------------------------------------------------------
INSERT INTO [PTH_Orders] ([OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('ORD-2026-000001', 1, 1, '2026-08-20 08:30:00', 'Published', 500.00, 0.00, 500.00, 500.00, 0.00, 'Paid', 'Demo patient case 1', '2026-08-20 08:30:00', 'reception');

INSERT INTO [PTH_OrderDetails] ([OrderId], [TestId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status]) VALUES
(1, 1, 1, 350.00, 0.00, 350.00, 1, 'Completed'),
(1, 2, 1, 150.00, 0.00, 150.00, 1, 'Completed');

INSERT INTO [PTH_Samples] ([SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('SMP-2026-000001', 1, 1, 1, 'BAR-2026-000001', '2026-08-20 09:00:00', 3, '2026-08-20 09:30:00', 3, 'Received', 'Vial received cold', '2026-08-20 08:30:00', 'reception');

INSERT INTO [PTH_SampleTests] ([SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn]) VALUES
(1, 1, 1, 'Completed', 3, '2026-08-20 09:30:00', '2026-08-20 10:30:00'),
(1, 2, 2, 'Completed', 3, '2026-08-20 09:30:00', '2026-08-20 10:30:00');

-- Results entry
INSERT INTO [PTH_TestResults] ([SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn]) VALUES
(1, 1, '14.5', 14.5, 'Normal', '13.0 - 17.0', 'g/dL', 3, '2026-08-20 10:30:00', 4, '2026-08-20 11:30:00'),
(1, 2, '5.5', 5.5, 'Normal', '4.0 - 11.0', '/µL', 3, '2026-08-20 10:30:00', 4, '2026-08-20 11:30:00'),
(1, 3, '2.8', 2.8, 'Normal', '1.5 - 4.5', 'lakh/µL', 3, '2026-08-20 10:30:00', 4, '2026-08-20 11:30:00'),
(2, 4, '95.0', 95.0, 'Normal', '70 - 100', 'mg/dL', 3, '2026-08-20 10:30:00', 4, '2026-08-20 11:30:00');

INSERT INTO [PTH_Invoices] ([InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy]) VALUES
('INV-2026-000001', 1, 1, '2026-08-20 08:30:00', 500.00, 0.00, 0.00, 500.00, 500.00, 0.00, 'Paid', '2026-08-20 08:30:00', 'reception');

INSERT INTO [PTH_Payments] ([InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [ReceivedBy], [CreatedOn], [CreatedBy]) VALUES
(1, 'PAY-2026-000001', '2026-08-20 08:35:00', 500.00, 'UPI', 'TXN9812345', 'reception', '2026-08-20 08:35:00', 'reception');

INSERT INTO [PTH_Reports] ([ReportNumber], [OrderId], [PatientId], [ReportStatus], [ReportDate], [VerifiedById], [VerifiedOn], [PublishedOn], [PdfFilePath], [VersionNumber], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('REP-2026-000001', 1, 1, 'Published', '2026-08-20 11:30:00', 4, '2026-08-20 11:30:00', '2026-08-20 11:35:00', '/uploads/Report_REP-2026-000001_V1.pdf', 1, 'Report verified successfully.', '2026-08-20 11:30:00', 'pathologist');

INSERT INTO [PTH_ReportDetails] ([ReportId], [TestId], [TestName], [Interpretation], [Remarks], [CreatedOn], [CreatedBy]) VALUES
(1, 1, 'Complete Blood Count', 'Blood parameters are well within normal reference ranges.', NULL, '2026-08-20 11:30:00', 'pathologist'),
(1, 2, 'Blood Sugar Fasting', 'Glucose index indicates normal fasting metabolic state.', NULL, '2026-08-20 11:30:00', 'pathologist');


-- -------------------------------------------------------------
-- DEMO CASE 2: Patient Ananya Sen (PatientId = 2) - Under Verification
-- -------------------------------------------------------------
INSERT INTO [PTH_Orders] ([OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('ORD-2026-000002', 2, 2, '2026-08-21 09:15:00', 'UnderVerification', 350.00, 0.00, 350.00, 350.00, 0.00, 'Paid', 'Urgent review requested', '2026-08-21 09:15:00', 'reception');

INSERT INTO [PTH_OrderDetails] ([OrderId], [TestId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status]) VALUES
(2, 1, 1, 350.00, 0.00, 350.00, 1, 'Completed');

INSERT INTO [PTH_Samples] ([SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('SMP-2026-000002', 2, 2, 1, 'BAR-2026-000002', '2026-08-21 09:30:00', 3, '2026-08-21 10:00:00', 3, 'Received', NULL, '2026-08-21 09:15:00', 'reception');

INSERT INTO [PTH_SampleTests] ([SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn]) VALUES
(2, 3, 1, 'Completed', 3, '2026-08-21 10:00:00', '2026-08-21 11:00:00');

-- Result values (Anemic patient!)
INSERT INTO [PTH_TestResults] ([SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [EnteredById], [EnteredOn]) VALUES
(3, 1, '9.2', 9.2, 'Low', '12.0 - 15.0', 'g/dL', 3, '2026-08-21 11:00:00'),
(3, 2, '6.1', 6.1, 'Normal', '4.0 - 11.0', '/µL', 3, '2026-08-21 11:00:00'),
(3, 3, '2.1', 2.1, 'Normal', '1.5 - 4.5', 'lakh/µL', 3, '2026-08-21 11:00:00');

INSERT INTO [PTH_Invoices] ([InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy]) VALUES
('INV-2026-000002', 2, 2, '2026-08-21 09:15:00', 350.00, 0.00, 0.00, 350.00, 350.00, 0.00, 'Paid', '2026-08-21 09:15:00', 'reception');

INSERT INTO [PTH_Payments] ([InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [ReceivedBy], [CreatedOn], [CreatedBy]) VALUES
(2, 'PAY-2026-000002', '2026-08-21 09:20:00', 350.00, 'Card', 'TXN5432198', 'reception', '2026-08-21 09:20:00', 'reception');

-- Report is under verification state (ReportStatus = UnderReview)
INSERT INTO [PTH_Reports] ([ReportNumber], [OrderId], [PatientId], [ReportStatus], [ReportDate], [VersionNumber], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('REP-2026-000002', 2, 2, 'UnderReview', '2026-08-21 11:05:00', 1, 'Needs validation check', '2026-08-21 11:05:00', 'technician');


-- -------------------------------------------------------------
-- DEMO CASE 3: Patient Rohan Mehta (PatientId = 3) - Registered, Pending Sample
-- -------------------------------------------------------------
INSERT INTO [PTH_Orders] ([OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('ORD-2026-000003', 3, 1, '2026-08-22 08:00:00', 'Registered', 200.00, 0.00, 200.00, 100.00, 100.00, 'PartiallyPaid', 'Out of town collection', '2026-08-22 08:00:00', 'reception');

INSERT INTO [PTH_OrderDetails] ([OrderId], [TestId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status]) VALUES
(3, 4, 1, 200.00, 0.00, 200.00, 1, 'Pending');

INSERT INTO [PTH_Samples] ([SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [SampleStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('SMP-2026-000003', 3, 3, 2, 'BAR-PENDING-03', 'Pending', NULL, '2026-08-22 08:00:00', 'reception');

INSERT INTO [PTH_Invoices] ([InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy]) VALUES
('INV-2026-000003', 3, 3, '2026-08-22 08:00:00', 200.00, 0.00, 0.00, 200.00, 100.00, 100.00, 'PartiallyPaid', '2026-08-22 08:00:00', 'reception');

INSERT INTO [PTH_Payments] ([InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [ReceivedBy], [CreatedOn], [CreatedBy]) VALUES
(3, 'PAY-2026-000003', '2026-08-22 08:05:00', 100.00, 'Cash', NULL, 'reception', '2026-08-22 08:05:00', 'reception');


-- -------------------------------------------------------------
-- DEMO CASE 4: Patient Sanjana Joshi (PatientId = 4) - Registered, Processing
-- -------------------------------------------------------------
INSERT INTO [PTH_Orders] ([OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('ORD-2026-000004', 4, 2, '2026-08-22 10:15:00', 'Registered', 150.00, 0.00, 150.00, 150.00, 0.00, 'Paid', NULL, '2026-08-22 10:15:00', 'reception');

INSERT INTO [PTH_OrderDetails] ([OrderId], [TestId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status]) VALUES
(4, 2, 1, 150.00, 0.00, 150.00, 1, 'In-Progress');

INSERT INTO [PTH_Samples] ([SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('SMP-2026-000004', 4, 4, 1, 'BAR-2026-000004', '2026-08-22 10:45:00', 3, '2026-08-22 11:15:00', 3, 'Received', NULL, '2026-08-22 10:15:00', 'reception');

INSERT INTO [PTH_SampleTests] ([SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn]) VALUES
(4, 4, 2, 'In-Progress', 3, '2026-08-22 11:30:00');

INSERT INTO [PTH_Invoices] ([InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy]) VALUES
('INV-2026-000004', 4, 4, '2026-08-22 10:15:00', 150.00, 0.00, 0.00, 150.00, 150.00, 0.00, 'Paid', '2026-08-22 10:15:00', 'reception');

INSERT INTO [PTH_Payments] ([InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [ReceivedBy], [CreatedOn], [CreatedBy]) VALUES
(4, 'PAY-2026-000004', '2026-08-22 10:20:00', 150.00, 'UPI', 'TXN321654', 'reception', '2026-08-22 10:20:00', 'reception');


-- -------------------------------------------------------------
-- DEMO CASE 5: Patient Vijay Singh (PatientId = 5) - Published Report (Critical sugar!)
-- -------------------------------------------------------------
INSERT INTO [PTH_Orders] ([OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('ORD-2026-000005', 5, 1, '2026-08-19 07:00:00', 'Published', 900.00, 0.00, 900.00, 900.00, 0.00, 'Paid', 'Diabetic follow-up', '2026-08-19 07:00:00', 'reception');

INSERT INTO [PTH_OrderDetails] ([OrderId], [TestId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status]) VALUES
(5, 2, 1, 150.00, 0.00, 150.00, 1, 'Completed'),
(5, 3, 1, 750.00, 0.00, 750.00, 1, 'Completed');

INSERT INTO [PTH_Samples] ([SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('SMP-2026-000005', 5, 5, 1, 'BAR-2026-000005', '2026-08-19 07:30:00', 3, '2026-08-19 08:00:00', 3, 'Received', NULL, '2026-08-19 07:00:00', 'reception'),
('SMP-2026-000006', 5, 5, 3, 'BAR-2026-000006', '2026-08-19 07:30:00', 3, '2026-08-19 08:00:00', 3, 'Received', NULL, '2026-08-19 07:00:00', 'reception');

INSERT INTO [PTH_SampleTests] ([SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn]) VALUES
(5, 5, 2, 'Completed', 3, '2026-08-19 08:00:00', '2026-08-19 09:30:00'),
(6, 6, 3, 'Completed', 3, '2026-08-19 08:00:00', '2026-08-19 09:30:00');

-- Result values (Extremely high fasting sugar, Bilirubin high!)
INSERT INTO [PTH_TestResults] ([SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn]) VALUES
(5, 4, '362.0', 362.0, 'Critical', '70 - 100', 'mg/dL', 3, '2026-08-19 09:30:00', 4, '2026-08-19 10:30:00'),
(6, 5, '4.2', 4.2, 'High', '0.1 - 1.2', 'mg/dL', 3, '2026-08-19 09:30:00', 4, '2026-08-19 10:30:00'),
(6, 6, '185.0', 185.0, 'High', '5 - 50', 'U/L', 3, '2026-08-19 09:30:00', 4, '2026-08-19 10:30:00');

INSERT INTO [PTH_Invoices] ([InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy]) VALUES
('INV-2026-000005', 5, 5, '2026-08-19 07:00:00', 900.00, 0.00, 0.00, 900.00, 900.00, 0.00, 'Paid', '2026-08-19 07:00:00', 'reception');

INSERT INTO [PTH_Payments] ([InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [ReceivedBy], [CreatedOn], [CreatedBy]) VALUES
(5, 'PAY-2026-000005', '2026-08-19 07:05:00', 900.00, 'UPI', 'TXN7654321', 'reception', '2026-08-19 07:05:00', 'reception');

INSERT INTO [PTH_Reports] ([ReportNumber], [OrderId], [PatientId], [ReportStatus], [ReportDate], [VerifiedById], [VerifiedOn], [PublishedOn], [PdfFilePath], [VersionNumber], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('REP-2026-000005', 5, 5, 'Published', '2026-08-19 10:30:00', 4, '2026-08-19 10:30:00', '2026-08-19 10:35:00', '/uploads/Report_REP-2026-000005_V1.pdf', 1, 'Critical values flagged.', '2026-08-19 10:30:00', 'pathologist');

INSERT INTO [PTH_ReportDetails] ([ReportId], [TestId], [TestName], [Interpretation], [Remarks], [CreatedOn], [CreatedBy]) VALUES
(3, 2, 'Blood Sugar Fasting', 'CRITICAL ALERT: Severely elevated fasting blood sugar levels observed. Clinical correlation and immediate physician consultation advised.', NULL, '2026-08-19 10:30:00', 'pathologist'),
(3, 3, 'Liver Function Test', 'Elevated Bilirubin Total and SGPT levels suggest hepatocellular injury / jaundice.', NULL, '2026-08-19 10:30:00', 'pathologist');
GO


-- -------------------------------------------------------------
-- AUXILIARY DEMO DATA (Home visits, Audits logs)
-- -------------------------------------------------------------
-- HomeCollections
INSERT INTO [PTH_HomeCollections] ([RequestNumber], [PatientId], [OrderId], [Address], [City], [Pincode], [RequestedDate], [RequestedTime], [AssignedAgentId], [Status], [Remarks], [CreatedOn], [CreatedBy]) VALUES
('HC-2026-000001', 3, 3, 'Flat 303, Tulip Residency, Sector 20', 'Navi Mumbai', '400703', '2026-08-23 00:00:00', '06:00 AM - 08:00 AM', 6, 'OnTheWay', 'Assigned agent Amit Verma', '2026-08-22 08:05:00', 'reception'),
('HC-2026-000002', 4, 4, 'Flat 404, Sunshine CHS, Sector 21', 'Navi Mumbai', '400703', '2026-08-22 00:00:00', '08:00 AM - 10:00 AM', 6, 'SampleCollected', 'Vials received at central laboratory Desk', '2026-08-22 10:20:00', 'reception');

-- AuditLogs
INSERT INTO [PTH_AuditLogs] ([UserId], [ModuleName], [Action], [TableName], [RecordId], [OldValues], [NewValues], [IPAddress], [UserAgent], [CreatedOn]) VALUES
(2, 'Orders', 'INSERT', 'PTH_Orders', 1, NULL, '{"OrderId":1,"TotalAmount":500.00,"PaymentStatus":"Paid"}', '::1', 'Mozilla/5.0', '2026-08-20 08:30:00'),
(3, 'Laboratory', 'INSERT', 'PTH_TestResults', 1, NULL, '{"SampleId":1,"Parameter":"HB","Value":"14.5"}', '::1', 'Mozilla/5.0', '2026-08-20 10:30:00'),
(4, 'Pathology', 'UPDATE', 'PTH_Reports', 1, NULL, '{"ReportId":1,"ReportStatus":"Published","VerifiedBy":4}', '::1', 'Mozilla/5.0', '2026-08-20 11:35:00'),
(2, 'Orders', 'INSERT', 'PTH_Orders', 2, NULL, '{"OrderId":2,"TotalAmount":350.00,"PaymentStatus":"Paid"}', '::1', 'Mozilla/5.0', '2026-08-21 09:15:00'),
(3, 'Laboratory', 'INSERT', 'PTH_TestResults', 2, NULL, '{"SampleId":2,"Parameter":"HB","Value":"9.2"}', '::1', 'Mozilla/5.0', '2026-08-21 11:00:00'),
(2, 'Orders', 'INSERT', 'PTH_Orders', 3, NULL, '{"OrderId":3,"TotalAmount":200.00,"PaymentStatus":"Partial"}', '::1', 'Mozilla/5.0', '2026-08-22 08:00:00'),
(2, 'Orders', 'INSERT', 'PTH_Orders', 5, NULL, '{"OrderId":5,"TotalAmount":900.00,"PaymentStatus":"Paid"}', '::1', 'Mozilla/5.0', '2026-08-19 07:00:00');
GO

PRINT 'RWD database schema generated and seeded successfully. Central staff logins are active.';
GO
