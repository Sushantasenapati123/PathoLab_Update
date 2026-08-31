USE [chikun1]
GO
/****** Object:  Table [dbo].[PTH_AuditLogs]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_AuditLogs](
	[AuditLogId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ModuleName] [nvarchar](50) NOT NULL,
	[Action] [nvarchar](50) NOT NULL,
	[TableName] [nvarchar](50) NOT NULL,
	[RecordId] [int] NOT NULL,
	[OldValues] [nvarchar](max) NULL,
	[NewValues] [nvarchar](max) NULL,
	[IPAddress] [nvarchar](50) NULL,
	[UserAgent] [nvarchar](250) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PTH_AuditLogs] PRIMARY KEY CLUSTERED 
(
	[AuditLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Departments]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Departments](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentCode] [nvarchar](20) NOT NULL,
	[DepartmentName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Departments] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Doctors]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Doctors](
	[DoctorId] [int] IDENTITY(1,1) NOT NULL,
	[DoctorCode] [nvarchar](20) NOT NULL,
	[DoctorName] [nvarchar](150) NOT NULL,
	[Qualification] [nvarchar](100) NULL,
	[Specialization] [nvarchar](100) NULL,
	[HospitalClinicName] [nvarchar](200) NULL,
	[Mobile] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [nvarchar](250) NULL,
	[CommissionType] [nvarchar](20) NOT NULL,
	[CommissionValue] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Doctors] PRIMARY KEY CLUSTERED 
(
	[DoctorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_HomeCollections]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_HomeCollections](
	[HomeCollectionId] [int] IDENTITY(1,1) NOT NULL,
	[RequestNumber] [nvarchar](30) NOT NULL,
	[PatientId] [int] NOT NULL,
	[OrderId] [int] NULL,
	[Address] [nvarchar](250) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[Pincode] [nvarchar](10) NOT NULL,
	[RequestedDate] [datetime2](7) NOT NULL,
	[RequestedTime] [nvarchar](100) NOT NULL,
	[AssignedAgentId] [int] NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CollectionDateTime] [datetime2](7) NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_HomeCollections] PRIMARY KEY CLUSTERED 
(
	[HomeCollectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Invoices]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Invoices](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [nvarchar](30) NOT NULL,
	[OrderId] [int] NOT NULL,
	[PatientId] [int] NOT NULL,
	[InvoiceDate] [datetime2](7) NOT NULL,
	[GrossAmount] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[TaxAmount] [decimal](18, 2) NOT NULL,
	[NetAmount] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[DueAmount] [decimal](18, 2) NOT NULL,
	[InvoiceStatus] [nvarchar](20) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Invoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Notifications]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Notifications](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[NotificationType] [nvarchar](20) NOT NULL,
	[Recipient] [nvarchar](150) NOT NULL,
	[Subject] [nvarchar](150) NULL,
	[Message] [nvarchar](max) NOT NULL,
	[ReferenceType] [nvarchar](50) NULL,
	[ReferenceId] [int] NULL,
	[Status] [nvarchar](20) NOT NULL,
	[SentOn] [datetime2](7) NULL,
	[FailureReason] [nvarchar](500) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Notifications] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_OrderDetails]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_OrderDetails](
	[OrderDetailId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[TestId] [int] NULL,
	[PackageId] [int] NULL,
	[Quantity] [int] NOT NULL,
	[Rate] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[SampleRequired] [bit] NOT NULL,
	[Status] [nvarchar](30) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Orders]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [nvarchar](30) NOT NULL,
	[PatientId] [int] NOT NULL,
	[DoctorId] [int] NULL,
	[OrderDate] [datetime2](7) NOT NULL,
	[OrderStatus] [nvarchar](30) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[NetAmount] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[DueAmount] [decimal](18, 2) NOT NULL,
	[PaymentStatus] [nvarchar](20) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_PackageTests]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_PackageTests](
	[PackageTestId] [int] IDENTITY(1,1) NOT NULL,
	[PackageId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_PackageTests] PRIMARY KEY CLUSTERED 
(
	[PackageTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Patients]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Patients](
	[PatientId] [int] IDENTITY(1,1) NOT NULL,
	[PatientCode] [nvarchar](20) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Gender] [nvarchar](20) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Age] [int] NOT NULL,
	[BloodGroup] [nvarchar](10) NULL,
	[Mobile] [nvarchar](20) NOT NULL,
	[AlternateMobile] [nvarchar](15) NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [nvarchar](250) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Pincode] [nvarchar](10) NULL,
	[EmergencyContactName] [nvarchar](100) NULL,
	[EmergencyContactMobile] [nvarchar](15) NULL,
	[IdentityType] [nvarchar](50) NULL,
	[IdentityNumber] [nvarchar](50) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Patients] PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Payments]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Payments](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceId] [int] NOT NULL,
	[PaymentNumber] [nvarchar](30) NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[PaymentMode] [nvarchar](50) NOT NULL,
	[TransactionReference] [nvarchar](100) NULL,
	[Remarks] [nvarchar](250) NULL,
	[ReceivedBy] [nvarchar](100) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Payments] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Permissions]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Permissions](
	[PermissionId] [int] IDENTITY(1,1) NOT NULL,
	[PermissionCode] [nvarchar](50) NOT NULL,
	[PermissionName] [nvarchar](100) NOT NULL,
	[ModuleName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Permissions] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_ReportDetails]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_ReportDetails](
	[ReportDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ReportId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
	[TestName] [nvarchar](150) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[Interpretation] [nvarchar](max) NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_ReportDetails] PRIMARY KEY CLUSTERED 
(
	[ReportDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Reports]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Reports](
	[ReportId] [int] IDENTITY(1,1) NOT NULL,
	[ReportNumber] [nvarchar](30) NOT NULL,
	[OrderId] [int] NOT NULL,
	[PatientId] [int] NOT NULL,
	[ReportStatus] [nvarchar](20) NOT NULL,
	[ReportDate] [datetime2](7) NOT NULL,
	[VerifiedById] [int] NULL,
	[VerifiedOn] [datetime2](7) NULL,
	[PublishedOn] [datetime2](7) NULL,
	[PdfFilePath] [nvarchar](500) NULL,
	[VersionNumber] [int] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Reports] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_RolePermissions]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_RolePermissions](
	[RolePermissionId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[RolePermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Roles]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Samples]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Samples](
	[SampleId] [int] IDENTITY(1,1) NOT NULL,
	[SampleNumber] [nvarchar](30) NOT NULL,
	[OrderId] [int] NOT NULL,
	[PatientId] [int] NOT NULL,
	[SampleTypeId] [int] NOT NULL,
	[Barcode] [nvarchar](50) NOT NULL,
	[CollectionDateTime] [datetime2](7) NULL,
	[CollectedById] [int] NULL,
	[ReceivedDateTime] [datetime2](7) NULL,
	[ReceivedById] [int] NULL,
	[SampleStatus] [nvarchar](20) NOT NULL,
	[RejectionReason] [nvarchar](250) NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Samples] PRIMARY KEY CLUSTERED 
(
	[SampleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_SampleTests]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_SampleTests](
	[SampleTestId] [int] IDENTITY(1,1) NOT NULL,
	[SampleId] [int] NOT NULL,
	[OrderDetailId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
	[Status] [nvarchar](30) NOT NULL,
	[AssignedToId] [int] NULL,
	[StartedOn] [datetime2](7) NULL,
	[CompletedOn] [datetime2](7) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_SampleTests] PRIMARY KEY CLUSTERED 
(
	[SampleTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_SampleTypes]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_SampleTypes](
	[SampleTypeId] [int] IDENTITY(1,1) NOT NULL,
	[SampleTypeCode] [nvarchar](20) NOT NULL,
	[SampleTypeName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_SampleTypes] PRIMARY KEY CLUSTERED 
(
	[SampleTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_TestPackages]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_TestPackages](
	[PackageId] [int] IDENTITY(1,1) NOT NULL,
	[PackageCode] [nvarchar](30) NOT NULL,
	[PackageName] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[OriginalPrice] [decimal](18, 2) NOT NULL,
	[PackagePrice] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_TestPackages] PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_TestParameters]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_TestParameters](
	[TestParameterId] [int] IDENTITY(1,1) NOT NULL,
	[TestId] [int] NOT NULL,
	[ParameterCode] [nvarchar](50) NOT NULL,
	[ParameterName] [nvarchar](150) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[ResultType] [nvarchar](50) NOT NULL,
	[Unit] [nvarchar](50) NULL,
	[MaleMin] [decimal](18, 4) NULL,
	[MaleMax] [decimal](18, 4) NULL,
	[FemaleMin] [decimal](18, 4) NULL,
	[FemaleMax] [decimal](18, 4) NULL,
	[ChildMin] [decimal](18, 4) NULL,
	[ChildMax] [decimal](18, 4) NULL,
	[CriticalLow] [decimal](18, 4) NULL,
	[CriticalHigh] [decimal](18, 4) NULL,
	[DefaultReferenceRange] [nvarchar](250) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_TestParameters] PRIMARY KEY CLUSTERED 
(
	[TestParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_TestResultHistory]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_TestResultHistory](
	[HistoryId] [int] IDENTITY(1,1) NOT NULL,
	[TestResultId] [int] NOT NULL,
	[OldValue] [nvarchar](200) NULL,
	[NewValue] [nvarchar](200) NULL,
	[OldStatus] [nvarchar](20) NULL,
	[NewStatus] [nvarchar](20) NULL,
	[Reason] [nvarchar](250) NOT NULL,
	[ChangedById] [int] NOT NULL,
	[ChangedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PTH_TestResultHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_TestResults]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_TestResults](
	[TestResultId] [int] IDENTITY(1,1) NOT NULL,
	[SampleTestId] [int] NOT NULL,
	[TestParameterId] [int] NOT NULL,
	[ResultValue] [nvarchar](200) NULL,
	[ResultNumericValue] [decimal](18, 4) NULL,
	[ResultStatus] [nvarchar](20) NOT NULL,
	[ReferenceRange] [nvarchar](200) NULL,
	[Unit] [nvarchar](50) NULL,
	[Remarks] [nvarchar](500) NULL,
	[EnteredById] [int] NULL,
	[EnteredOn] [datetime2](7) NULL,
	[VerifiedById] [int] NULL,
	[VerifiedOn] [datetime2](7) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_TestResults] PRIMARY KEY CLUSTERED 
(
	[TestResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Tests]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Tests](
	[TestId] [int] IDENTITY(1,1) NOT NULL,
	[TestCode] [nvarchar](20) NOT NULL,
	[TestName] [nvarchar](150) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[SampleTypeId] [int] NOT NULL,
	[TestType] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[TATMinutes] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Tests] PRIMARY KEY CLUSTERED 
(
	[TestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTH_Users]    Script Date: 22-08-2026 16:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTH_Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Mobile] [nvarchar](15) NOT NULL,
	[RoleId] [int] NOT NULL,
	[LastLoginDate] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[DeletedFlag] [bit] NOT NULL,
 CONSTRAINT [PK_PTH_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[PTH_AuditLogs] ON 

INSERT [dbo].[PTH_AuditLogs] ([AuditLogId], [UserId], [ModuleName], [Action], [TableName], [RecordId], [OldValues], [NewValues], [IPAddress], [UserAgent], [CreatedOn]) VALUES (1, 2, N'Orders', N'INSERT', N'PTH_Orders', 1, NULL, N'{"OrderId":1,"TotalAmount":500.00,"PaymentStatus":"Paid"}', N'::1', N'Mozilla/5.0', CAST(N'2026-08-20T08:30:00.0000000' AS DateTime2))
INSERT [dbo].[PTH_AuditLogs] ([AuditLogId], [UserId], [ModuleName], [Action], [TableName], [RecordId], [OldValues], [NewValues], [IPAddress], [UserAgent], [CreatedOn]) VALUES (2, 3, N'Laboratory', N'INSERT', N'PTH_TestResults', 1, NULL, N'{"SampleId":1,"Parameter":"HB","Value":"14.5"}', N'::1', N'Mozilla/5.0', CAST(N'2026-08-20T10:30:00.0000000' AS DateTime2))
INSERT [dbo].[PTH_AuditLogs] ([AuditLogId], [UserId], [ModuleName], [Action], [TableName], [RecordId], [OldValues], [NewValues], [IPAddress], [UserAgent], [CreatedOn]) VALUES (3, 4, N'Pathology', N'UPDATE', N'PTH_Reports', 1, NULL, N'{"ReportId":1,"ReportStatus":"Published","VerifiedBy":4}', N'::1', N'Mozilla/5.0', CAST(N'2026-08-20T11:35:00.0000000' AS DateTime2))
INSERT [dbo].[PTH_AuditLogs] ([AuditLogId], [UserId], [ModuleName], [Action], [TableName], [RecordId], [OldValues], [NewValues], [IPAddress], [UserAgent], [CreatedOn]) VALUES (4, 2, N'Orders', N'INSERT', N'PTH_Orders', 2, NULL, N'{"OrderId":2,"TotalAmount":350.00,"PaymentStatus":"Paid"}', N'::1', N'Mozilla/5.0', CAST(N'2026-08-21T09:15:00.0000000' AS DateTime2))
INSERT [dbo].[PTH_AuditLogs] ([AuditLogId], [UserId], [ModuleName], [Action], [TableName], [RecordId], [OldValues], [NewValues], [IPAddress], [UserAgent], [CreatedOn]) VALUES (5, 3, N'Laboratory', N'INSERT', N'PTH_TestResults', 2, NULL, N'{"SampleId":2,"Parameter":"HB","Value":"9.2"}', N'::1', N'Mozilla/5.0', CAST(N'2026-08-21T11:00:00.0000000' AS DateTime2))
INSERT [dbo].[PTH_AuditLogs] ([AuditLogId], [UserId], [ModuleName], [Action], [TableName], [RecordId], [OldValues], [NewValues], [IPAddress], [UserAgent], [CreatedOn]) VALUES (6, 2, N'Orders', N'INSERT', N'PTH_Orders', 3, NULL, N'{"OrderId":3,"TotalAmount":200.00,"PaymentStatus":"Partial"}', N'::1', N'Mozilla/5.0', CAST(N'2026-08-22T08:00:00.0000000' AS DateTime2))
INSERT [dbo].[PTH_AuditLogs] ([AuditLogId], [UserId], [ModuleName], [Action], [TableName], [RecordId], [OldValues], [NewValues], [IPAddress], [UserAgent], [CreatedOn]) VALUES (7, 2, N'Orders', N'INSERT', N'PTH_Orders', 5, NULL, N'{"OrderId":5,"TotalAmount":900.00,"PaymentStatus":"Paid"}', N'::1', N'Mozilla/5.0', CAST(N'2026-08-19T07:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[PTH_AuditLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Departments] ON 

INSERT [dbo].[PTH_Departments] ([DepartmentId], [DepartmentCode], [DepartmentName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'HEM', N'Hematology', N'Blood cell assays', 1, CAST(N'2026-08-22T10:02:41.2166667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Departments] ([DepartmentId], [DepartmentCode], [DepartmentName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'BIO', N'Biochemistry', N'Serology and metabolic assays', 1, CAST(N'2026-08-22T10:02:41.2166667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Departments] ([DepartmentId], [DepartmentCode], [DepartmentName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'CLP', N'Clinical Pathology', N'Fluids and Urine Routine analysis', 1, CAST(N'2026-08-22T10:02:41.2166667' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Departments] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Doctors] ON 

INSERT [dbo].[PTH_Doctors] ([DoctorId], [DoctorCode], [DoctorName], [Qualification], [Specialization], [HospitalClinicName], [Mobile], [Email], [Address], [CommissionType], [CommissionValue], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'DOC-000001', N'Dr. Rajesh Nair', N'MBBS, MD', N'General Medicine', N'Apex Multispeciality Care', N'9988776601', N'dr.rajesh@apexcare.com', N'Apex Arcade, Suite 10', N'Percentage', CAST(10.00 AS Decimal(18, 2)), 1, CAST(N'2026-08-22T10:02:41.2966667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Doctors] ([DoctorId], [DoctorCode], [DoctorName], [Qualification], [Specialization], [HospitalClinicName], [Mobile], [Email], [Address], [CommissionType], [CommissionValue], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'DOC-000002', N'Dr. Priya Desai', N'MBBS, DNB', N'Gynecology', N'Desai Womens Clinic', N'9988776602', N'dr.desai@clinic.com', N'Desai Chambers, Suite 2', N'Percentage', CAST(10.00 AS Decimal(18, 2)), 1, CAST(N'2026-08-22T10:02:41.2966667' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Doctors] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_HomeCollections] ON 

INSERT [dbo].[PTH_HomeCollections] ([HomeCollectionId], [RequestNumber], [PatientId], [OrderId], [Address], [City], [Pincode], [RequestedDate], [RequestedTime], [AssignedAgentId], [Status], [CollectionDateTime], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'HC-2026-000001', 3, 3, N'Flat 303, Tulip Residency, Sector 20', N'Navi Mumbai', N'400703', CAST(N'2026-08-23T00:00:00.0000000' AS DateTime2), N'06:00 AM - 08:00 AM', 6, N'OnTheWay', NULL, N'Assigned agent Amit Verma', CAST(N'2026-08-22T08:05:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_HomeCollections] ([HomeCollectionId], [RequestNumber], [PatientId], [OrderId], [Address], [City], [Pincode], [RequestedDate], [RequestedTime], [AssignedAgentId], [Status], [CollectionDateTime], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'HC-2026-000002', 4, 4, N'Flat 404, Sunshine CHS, Sector 21', N'Navi Mumbai', N'400703', CAST(N'2026-08-22T00:00:00.0000000' AS DateTime2), N'08:00 AM - 10:00 AM', 6, N'SampleCollected', NULL, N'Vials received at central laboratory Desk', CAST(N'2026-08-22T10:20:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_HomeCollections] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Invoices] ON 

INSERT [dbo].[PTH_Invoices] ([InvoiceId], [InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'INV-2026-000001', 1, 1, CAST(N'2026-08-20T08:30:00.0000000' AS DateTime2), CAST(500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', CAST(N'2026-08-20T08:30:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Invoices] ([InvoiceId], [InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'INV-2026-000002', 2, 2, CAST(N'2026-08-21T09:15:00.0000000' AS DateTime2), CAST(350.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(350.00 AS Decimal(18, 2)), CAST(350.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', CAST(N'2026-08-21T09:15:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Invoices] ([InvoiceId], [InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'INV-2026-000003', 3, 3, CAST(N'2026-08-22T08:00:00.0000000' AS DateTime2), CAST(200.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), N'PartiallyPaid', CAST(N'2026-08-22T08:00:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Invoices] ([InvoiceId], [InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, N'INV-2026-000004', 4, 4, CAST(N'2026-08-22T10:15:00.0000000' AS DateTime2), CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', CAST(N'2026-08-22T10:15:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Invoices] ([InvoiceId], [InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, N'INV-2026-000005', 5, 5, CAST(N'2026-08-19T07:00:00.0000000' AS DateTime2), CAST(900.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(900.00 AS Decimal(18, 2)), CAST(900.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', CAST(N'2026-08-19T07:00:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Invoices] ([InvoiceId], [InvoiceNumber], [OrderId], [PatientId], [InvoiceDate], [GrossAmount], [DiscountAmount], [TaxAmount], [NetAmount], [PaidAmount], [DueAmount], [InvoiceStatus], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, N'INV-2026-000006', 6, 6, CAST(N'2026-08-22T10:18:04.8183278' AS DateTime2), CAST(1450.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1450.00 AS Decimal(18, 2)), CAST(1450.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', CAST(N'2026-08-22T10:18:04.8186928' AS DateTime2), N'System', CAST(N'2026-08-22T10:20:43.0655362' AS DateTime2), N'System', 0)
SET IDENTITY_INSERT [dbo].[PTH_Invoices] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_OrderDetails] ON 

INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, 1, 1, NULL, 1, CAST(350.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(350.00 AS Decimal(18, 2)), 1, N'Completed', CAST(N'2026-08-22T10:02:41.3200000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, 1, 2, NULL, 1, CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), 1, N'Completed', CAST(N'2026-08-22T10:02:41.3200000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, 2, 1, NULL, 1, CAST(350.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(350.00 AS Decimal(18, 2)), 1, N'Completed', CAST(N'2026-08-22T10:02:41.3700000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, 3, 4, NULL, 1, CAST(200.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), 1, N'Pending', CAST(N'2026-08-22T10:02:41.3933333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, 4, 2, NULL, 1, CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), 1, N'In-Progress', CAST(N'2026-08-22T10:02:41.4066667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, 5, 2, NULL, 1, CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), 1, N'Completed', CAST(N'2026-08-22T10:02:41.4200000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, 5, 3, NULL, 1, CAST(750.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(750.00 AS Decimal(18, 2)), 1, N'Completed', CAST(N'2026-08-22T10:02:41.4200000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (8, 6, 1, NULL, 1, CAST(350.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(350.00 AS Decimal(18, 2)), 1, N'Pending', CAST(N'2026-08-22T10:18:04.4846876' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (9, 6, 2, NULL, 1, CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), 1, N'Pending', CAST(N'2026-08-22T10:18:04.4846910' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (10, 6, 3, NULL, 1, CAST(750.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(750.00 AS Decimal(18, 2)), 1, N'Pending', CAST(N'2026-08-22T10:18:04.4846912' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_OrderDetails] ([OrderDetailId], [OrderId], [TestId], [PackageId], [Quantity], [Rate], [Discount], [Amount], [SampleRequired], [Status], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (11, 6, 4, NULL, 1, CAST(200.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), 1, N'Pending', CAST(N'2026-08-22T10:18:04.4846914' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Orders] ON 

INSERT [dbo].[PTH_Orders] ([OrderId], [OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'ORD-2026-000001', 1, 1, CAST(N'2026-08-20T08:30:00.0000000' AS DateTime2), N'Published', CAST(500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', N'Demo patient case 1', CAST(N'2026-08-20T08:30:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Orders] ([OrderId], [OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'ORD-2026-000002', 2, 2, CAST(N'2026-08-21T09:15:00.0000000' AS DateTime2), N'UnderVerification', CAST(350.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(350.00 AS Decimal(18, 2)), CAST(350.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', N'Urgent review requested', CAST(N'2026-08-21T09:15:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Orders] ([OrderId], [OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'ORD-2026-000003', 3, 1, CAST(N'2026-08-22T08:00:00.0000000' AS DateTime2), N'Registered', CAST(200.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), N'PartiallyPaid', N'Out of town collection', CAST(N'2026-08-22T08:00:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Orders] ([OrderId], [OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, N'ORD-2026-000004', 4, 2, CAST(N'2026-08-22T10:15:00.0000000' AS DateTime2), N'Registered', CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', NULL, CAST(N'2026-08-22T10:15:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Orders] ([OrderId], [OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, N'ORD-2026-000005', 5, 1, CAST(N'2026-08-19T07:00:00.0000000' AS DateTime2), N'Published', CAST(900.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(900.00 AS Decimal(18, 2)), CAST(900.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', N'Diabetic follow-up', CAST(N'2026-08-19T07:00:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Orders] ([OrderId], [OrderNumber], [PatientId], [DoctorId], [OrderDate], [OrderStatus], [TotalAmount], [DiscountAmount], [NetAmount], [PaidAmount], [DueAmount], [PaymentStatus], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, N'ORD-2026-000006', 6, 2, CAST(N'2026-08-22T10:18:04.4919313' AS DateTime2), N'SampleCollected', CAST(1450.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1450.00 AS Decimal(18, 2)), CAST(1450.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Paid', NULL, CAST(N'2026-08-22T10:18:04.4927101' AS DateTime2), N'System', CAST(N'2026-08-22T10:21:32.0104327' AS DateTime2), N'System', 0)
SET IDENTITY_INSERT [dbo].[PTH_Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_PackageTests] ON 

INSERT [dbo].[PTH_PackageTests] ([PackageTestId], [PackageId], [TestId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, 1, 1, CAST(N'2026-08-22T10:02:41.2800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_PackageTests] ([PackageTestId], [PackageId], [TestId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, 1, 2, CAST(N'2026-08-22T10:02:41.2800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_PackageTests] ([PackageTestId], [PackageId], [TestId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, 1, 3, CAST(N'2026-08-22T10:02:41.2800000' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_PackageTests] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Patients] ON 

INSERT [dbo].[PTH_Patients] ([PatientId], [PatientCode], [FirstName], [MiddleName], [LastName], [Gender], [DateOfBirth], [Age], [BloodGroup], [Mobile], [AlternateMobile], [Email], [Address], [City], [State], [Pincode], [EmergencyContactName], [EmergencyContactMobile], [IdentityType], [IdentityNumber], [Remarks], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'PAT-000001', N'Aarav', NULL, N'Sharma', N'Male', CAST(N'1990-05-15T00:00:00.0000000' AS DateTime2), 36, N'O+', N'9812345601', NULL, N'aarav.sharma@gmail.com', N'Flat 101, Galaxy Tower, Sector 15', N'Navi Mumbai', N'Maharashtra', N'400703', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2026-08-22T10:02:41.2900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Patients] ([PatientId], [PatientCode], [FirstName], [MiddleName], [LastName], [Gender], [DateOfBirth], [Age], [BloodGroup], [Mobile], [AlternateMobile], [Email], [Address], [City], [State], [Pincode], [EmergencyContactName], [EmergencyContactMobile], [IdentityType], [IdentityNumber], [Remarks], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'PAT-000002', N'Ananya', NULL, N'Sen', N'Female', CAST(N'1995-10-22T00:00:00.0000000' AS DateTime2), 30, N'A+', N'9812345602', NULL, N'ananya.sen@gmail.com', N'Flat 202, Regency Park, Sector 11', N'Navi Mumbai', N'Maharashtra', N'400703', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2026-08-22T10:02:41.2900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Patients] ([PatientId], [PatientCode], [FirstName], [MiddleName], [LastName], [Gender], [DateOfBirth], [Age], [BloodGroup], [Mobile], [AlternateMobile], [Email], [Address], [City], [State], [Pincode], [EmergencyContactName], [EmergencyContactMobile], [IdentityType], [IdentityNumber], [Remarks], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'PAT-000003', N'Rohan', NULL, N'Mehta', N'Male', CAST(N'1982-08-04T00:00:00.0000000' AS DateTime2), 44, N'B+', N'9812345603', NULL, N'rohan.mehta@gmail.com', N'Flat 303, Tulip Residency, Sector 20', N'Navi Mumbai', N'Maharashtra', N'400703', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2026-08-22T10:02:41.2900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Patients] ([PatientId], [PatientCode], [FirstName], [MiddleName], [LastName], [Gender], [DateOfBirth], [Age], [BloodGroup], [Mobile], [AlternateMobile], [Email], [Address], [City], [State], [Pincode], [EmergencyContactName], [EmergencyContactMobile], [IdentityType], [IdentityNumber], [Remarks], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, N'PAT-000004', N'Sanjana', NULL, N'Joshi', N'Female', CAST(N'1999-01-30T00:00:00.0000000' AS DateTime2), 27, N'O-', N'9812345604', NULL, N'sanjana.joshi@gmail.com', N'Flat 404, Sunshine CHS, Sector 21', N'Navi Mumbai', N'Maharashtra', N'400703', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2026-08-22T10:02:41.2900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Patients] ([PatientId], [PatientCode], [FirstName], [MiddleName], [LastName], [Gender], [DateOfBirth], [Age], [BloodGroup], [Mobile], [AlternateMobile], [Email], [Address], [City], [State], [Pincode], [EmergencyContactName], [EmergencyContactMobile], [IdentityType], [IdentityNumber], [Remarks], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, N'PAT-000005', N'Vijay', NULL, N'Singh', N'Male', CAST(N'1965-03-12T00:00:00.0000000' AS DateTime2), 61, N'AB+', N'9812345605', NULL, N'vijay.singh@gmail.com', N'Flat 505, Palm Beach View, Sector 4', N'Navi Mumbai', N'Maharashtra', N'400703', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2026-08-22T10:02:41.2900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Patients] ([PatientId], [PatientCode], [FirstName], [MiddleName], [LastName], [Gender], [DateOfBirth], [Age], [BloodGroup], [Mobile], [AlternateMobile], [Email], [Address], [City], [State], [Pincode], [EmergencyContactName], [EmergencyContactMobile], [IdentityType], [IdentityNumber], [Remarks], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, N'PAT-000006', N'Sushanta', NULL, N'Senapati', N'Male', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 0, N'A-', N'08658193889', NULL, N'sushantasenapati718@gmail.com', N'Kuliana b', N'Baripada', N'Odisha', N'757025', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2026-08-22T10:16:27.6448094' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Patients] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Payments] ON 

INSERT [dbo].[PTH_Payments] ([PaymentId], [InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [Remarks], [ReceivedBy], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, 1, N'PAY-2026-000001', CAST(N'2026-08-20T08:35:00.0000000' AS DateTime2), CAST(500.00 AS Decimal(18, 2)), N'UPI', N'TXN9812345', NULL, N'reception', CAST(N'2026-08-20T08:35:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Payments] ([PaymentId], [InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [Remarks], [ReceivedBy], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, 2, N'PAY-2026-000002', CAST(N'2026-08-21T09:20:00.0000000' AS DateTime2), CAST(350.00 AS Decimal(18, 2)), N'Card', N'TXN5432198', NULL, N'reception', CAST(N'2026-08-21T09:20:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Payments] ([PaymentId], [InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [Remarks], [ReceivedBy], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, 3, N'PAY-2026-000003', CAST(N'2026-08-22T08:05:00.0000000' AS DateTime2), CAST(100.00 AS Decimal(18, 2)), N'Cash', NULL, NULL, N'reception', CAST(N'2026-08-22T08:05:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Payments] ([PaymentId], [InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [Remarks], [ReceivedBy], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, 4, N'PAY-2026-000004', CAST(N'2026-08-22T10:20:00.0000000' AS DateTime2), CAST(150.00 AS Decimal(18, 2)), N'UPI', N'TXN321654', NULL, N'reception', CAST(N'2026-08-22T10:20:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Payments] ([PaymentId], [InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [Remarks], [ReceivedBy], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, 5, N'PAY-2026-000005', CAST(N'2026-08-19T07:05:00.0000000' AS DateTime2), CAST(900.00 AS Decimal(18, 2)), N'UPI', N'TXN7654321', NULL, N'reception', CAST(N'2026-08-19T07:05:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Payments] ([PaymentId], [InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [Remarks], [ReceivedBy], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, 6, N'PAY-2026-000006', CAST(N'2026-08-22T10:18:04.9042443' AS DateTime2), CAST(150.00 AS Decimal(18, 2)), N'Cash', NULL, NULL, N'System', CAST(N'2026-08-22T10:18:04.9044976' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Payments] ([PaymentId], [InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [Remarks], [ReceivedBy], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, 6, N'PAY-2026-000007', CAST(N'2026-08-22T10:18:51.3692155' AS DateTime2), CAST(300.00 AS Decimal(18, 2)), N'UPI', NULL, NULL, N'admin', CAST(N'2026-08-22T10:18:51.3694321' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Payments] ([PaymentId], [InvoiceId], [PaymentNumber], [PaymentDate], [Amount], [PaymentMode], [TransactionReference], [Remarks], [ReceivedBy], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (8, 6, N'PAY-2026-000008', CAST(N'2026-08-22T10:20:43.0654291' AS DateTime2), CAST(1000.00 AS Decimal(18, 2)), N'UPI', NULL, NULL, N'admin', CAST(N'2026-08-22T10:20:43.0654296' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Payments] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Permissions] ON 

INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'PATIENTS_VIEW', N'View Patients', N'Patient Management', N'View patient directories', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'PATIENTS_CREATE', N'Create Patient', N'Patient Management', N'Register new profile', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'PATIENTS_EDIT', N'Edit Patient', N'Patient Management', N'Edit profile items', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, N'PATIENTS_DELETE', N'Delete Patient', N'Patient Management', N'Delete profile', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, N'ORDERS_VIEW', N'View Orders', N'Order Management', N'View orders list', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, N'ORDERS_CREATE', N'Create Order', N'Order Management', N'Book new order case', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, N'SAMPLES_COLLECT', N'Collect Sample', N'Laboratory Operations', N'Vials draw details', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (8, N'SAMPLES_RECEIVE', N'Receive Sample', N'Laboratory Operations', N'Vials lab receipts', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (9, N'RESULTS_ENTER', N'Enter Results', N'Laboratory Operations', N'Observed values entry', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (10, N'REPORTS_VERIFY', N'Verify Reports', N'Pathology Sign-off', N'Pathologist review approval', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (11, N'REPORTS_PUBLISH', N'Publish Reports', N'Pathology Sign-off', N'Digital sign release', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (12, N'BILLING_VIEW', N'View Invoices', N'Billing & Finance', N'View invoices ledger', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (13, N'BILLING_PAY', N'Collect Payment', N'Billing & Finance', N'Collect balances receipt', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Permissions] ([PermissionId], [PermissionCode], [PermissionName], [ModuleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (14, N'ADMIN_USERS', N'Manage Users', N'System Admin', N'Manage staff login accounts', 1, CAST(N'2026-08-22T10:02:41.1633333' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Permissions] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_ReportDetails] ON 

INSERT [dbo].[PTH_ReportDetails] ([ReportDetailId], [ReportId], [TestId], [TestName], [DisplayOrder], [Interpretation], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, 1, 1, N'Complete Blood Count', 0, N'Blood parameters are well within normal reference ranges.', NULL, CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), N'pathologist', NULL, NULL, 0)
INSERT [dbo].[PTH_ReportDetails] ([ReportDetailId], [ReportId], [TestId], [TestName], [DisplayOrder], [Interpretation], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, 1, 2, N'Blood Sugar Fasting', 0, N'Glucose index indicates normal fasting metabolic state.', NULL, CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), N'pathologist', NULL, NULL, 0)
INSERT [dbo].[PTH_ReportDetails] ([ReportDetailId], [ReportId], [TestId], [TestName], [DisplayOrder], [Interpretation], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, 3, 2, N'Blood Sugar Fasting', 0, N'CRITICAL ALERT: Severely elevated fasting blood sugar levels observed. Clinical correlation and immediate physician consultation advised.', NULL, CAST(N'2026-08-19T10:30:00.0000000' AS DateTime2), N'pathologist', NULL, NULL, 0)
INSERT [dbo].[PTH_ReportDetails] ([ReportDetailId], [ReportId], [TestId], [TestName], [DisplayOrder], [Interpretation], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, 3, 3, N'Liver Function Test', 0, N'Elevated Bilirubin Total and SGPT levels suggest hepatocellular injury / jaundice.', NULL, CAST(N'2026-08-19T10:30:00.0000000' AS DateTime2), N'pathologist', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_ReportDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Reports] ON 

INSERT [dbo].[PTH_Reports] ([ReportId], [ReportNumber], [OrderId], [PatientId], [ReportStatus], [ReportDate], [VerifiedById], [VerifiedOn], [PublishedOn], [PdfFilePath], [VersionNumber], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'REP-2026-000001', 1, 1, N'Published', CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), CAST(N'2026-08-20T11:35:00.0000000' AS DateTime2), N'/uploads/Report_REP-2026-000001_V1.pdf', 1, N'Report verified successfully.', CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), N'pathologist', NULL, NULL, 0)
INSERT [dbo].[PTH_Reports] ([ReportId], [ReportNumber], [OrderId], [PatientId], [ReportStatus], [ReportDate], [VerifiedById], [VerifiedOn], [PublishedOn], [PdfFilePath], [VersionNumber], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'REP-2026-000002', 2, 2, N'UnderReview', CAST(N'2026-08-21T11:05:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 1, N'Needs validation check', CAST(N'2026-08-21T11:05:00.0000000' AS DateTime2), N'technician', NULL, NULL, 0)
INSERT [dbo].[PTH_Reports] ([ReportId], [ReportNumber], [OrderId], [PatientId], [ReportStatus], [ReportDate], [VerifiedById], [VerifiedOn], [PublishedOn], [PdfFilePath], [VersionNumber], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'REP-2026-000005', 5, 5, N'Published', CAST(N'2026-08-19T10:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-19T10:30:00.0000000' AS DateTime2), CAST(N'2026-08-19T10:35:00.0000000' AS DateTime2), N'/uploads/Report_REP-2026-000005_V1.pdf', 1, N'Critical values flagged.', CAST(N'2026-08-19T10:30:00.0000000' AS DateTime2), N'pathologist', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Reports] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_RolePermissions] ON 

INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, 1, 1, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, 1, 2, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, 1, 3, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, 1, 4, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, 1, 5, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, 1, 6, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, 1, 7, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (8, 1, 8, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (9, 1, 9, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (10, 1, 10, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (11, 1, 11, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (12, 1, 12, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (13, 1, 13, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (14, 1, 14, CAST(N'2026-08-22T10:02:41.1766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (15, 2, 1, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (16, 2, 2, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (17, 2, 3, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (18, 2, 4, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (19, 2, 5, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (20, 2, 6, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (21, 2, 7, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (22, 2, 8, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (23, 2, 9, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (24, 2, 10, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (25, 2, 11, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (26, 2, 12, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (27, 2, 13, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (28, 2, 14, CAST(N'2026-08-22T10:02:41.1800000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (29, 3, 1, CAST(N'2026-08-22T10:02:41.1866667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (30, 3, 2, CAST(N'2026-08-22T10:02:41.1866667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (31, 3, 3, CAST(N'2026-08-22T10:02:41.1866667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (32, 3, 5, CAST(N'2026-08-22T10:02:41.1866667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (33, 3, 6, CAST(N'2026-08-22T10:02:41.1866667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (34, 3, 12, CAST(N'2026-08-22T10:02:41.1866667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (35, 3, 13, CAST(N'2026-08-22T10:02:41.1866667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (36, 4, 1, CAST(N'2026-08-22T10:02:41.1900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (37, 4, 5, CAST(N'2026-08-22T10:02:41.1900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (38, 4, 7, CAST(N'2026-08-22T10:02:41.1900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (39, 4, 8, CAST(N'2026-08-22T10:02:41.1900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (40, 4, 9, CAST(N'2026-08-22T10:02:41.1900000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (41, 5, 1, CAST(N'2026-08-22T10:02:41.1966667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (42, 5, 5, CAST(N'2026-08-22T10:02:41.1966667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (43, 5, 9, CAST(N'2026-08-22T10:02:41.1966667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (44, 5, 10, CAST(N'2026-08-22T10:02:41.1966667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_RolePermissions] ([RolePermissionId], [RoleId], [PermissionId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (45, 5, 11, CAST(N'2026-08-22T10:02:41.1966667' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_RolePermissions] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Roles] ON 

INSERT [dbo].[PTH_Roles] ([RoleId], [RoleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'Super Admin', N'Full control and settings audit access', 1, CAST(N'2026-08-22T10:02:41.1533333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Roles] ([RoleId], [RoleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'Lab Admin', N'Test parameters settings configuration access', 1, CAST(N'2026-08-22T10:02:41.1533333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Roles] ([RoleId], [RoleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'Receptionist', N'Patients intake billing and cash counters', 1, CAST(N'2026-08-22T10:02:41.1533333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Roles] ([RoleId], [RoleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, N'Lab Technician', N'Vials collect, lab receipt, and results entry', 1, CAST(N'2026-08-22T10:02:41.1533333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Roles] ([RoleId], [RoleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, N'Pathologist', N'Awaiting review sign-off validations', 1, CAST(N'2026-08-22T10:02:41.1533333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Roles] ([RoleId], [RoleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, N'Accountant', N'Outstanding collections settlement audit', 1, CAST(N'2026-08-22T10:02:41.1533333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Roles] ([RoleId], [RoleName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, N'Collection Agent', N'Home visits blood samples draw', 1, CAST(N'2026-08-22T10:02:41.1533333' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Samples] ON 

INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'SMP-2026-000001', 1, 1, 1, N'BAR-2026-000001', CAST(N'2026-08-20T09:00:00.0000000' AS DateTime2), 3, CAST(N'2026-08-20T09:30:00.0000000' AS DateTime2), 3, N'Received', NULL, N'Vial received cold', CAST(N'2026-08-20T08:30:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'SMP-2026-000002', 2, 2, 1, N'BAR-2026-000002', CAST(N'2026-08-21T09:30:00.0000000' AS DateTime2), 3, CAST(N'2026-08-21T10:00:00.0000000' AS DateTime2), 3, N'Received', NULL, NULL, CAST(N'2026-08-21T09:15:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'SMP-2026-000003', 3, 3, 2, N'BAR-PENDING-03', NULL, NULL, NULL, NULL, N'Pending', NULL, NULL, CAST(N'2026-08-22T08:00:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, N'SMP-2026-000004', 4, 4, 1, N'BAR-2026-000004', CAST(N'2026-08-22T10:45:00.0000000' AS DateTime2), 3, CAST(N'2026-08-22T11:15:00.0000000' AS DateTime2), 3, N'Received', NULL, NULL, CAST(N'2026-08-22T10:15:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, N'SMP-2026-000005', 5, 5, 1, N'BAR-2026-000005', CAST(N'2026-08-19T07:30:00.0000000' AS DateTime2), 3, CAST(N'2026-08-19T08:00:00.0000000' AS DateTime2), 3, N'Received', NULL, NULL, CAST(N'2026-08-19T07:00:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, N'SMP-2026-000006', 5, 5, 3, N'BAR-2026-000006', CAST(N'2026-08-19T07:30:00.0000000' AS DateTime2), 3, CAST(N'2026-08-19T08:00:00.0000000' AS DateTime2), 3, N'Received', NULL, NULL, CAST(N'2026-08-19T07:00:00.0000000' AS DateTime2), N'reception', NULL, NULL, 0)
INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, N'SMP-2026-000007', 6, 6, 1, N'BAR-2026-658283', CAST(N'2026-08-22T10:21:32.0104269' AS DateTime2), 1, NULL, NULL, N'Collected', NULL, NULL, CAST(N'2026-08-22T10:18:04.6896680' AS DateTime2), N'System', CAST(N'2026-08-22T10:21:32.0104279' AS DateTime2), N'System', 0)
INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (8, N'SMP-2026-000008', 6, 6, 3, N'BAR-2026-601582', CAST(N'2026-08-22T10:21:29.0224026' AS DateTime2), 1, NULL, NULL, N'Collected', NULL, NULL, CAST(N'2026-08-22T10:18:04.6969270' AS DateTime2), N'System', CAST(N'2026-08-22T10:21:29.0224034' AS DateTime2), N'System', 0)
INSERT [dbo].[PTH_Samples] ([SampleId], [SampleNumber], [OrderId], [PatientId], [SampleTypeId], [Barcode], [CollectionDateTime], [CollectedById], [ReceivedDateTime], [ReceivedById], [SampleStatus], [RejectionReason], [Remarks], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (9, N'SMP-2026-000009', 6, 6, 2, N'BAR-2026-910696', CAST(N'2026-08-22T10:19:56.1574996' AS DateTime2), 1, NULL, NULL, N'Collected', NULL, NULL, CAST(N'2026-08-22T10:18:04.6971002' AS DateTime2), N'System', CAST(N'2026-08-22T10:19:56.1576938' AS DateTime2), N'System', 0)
SET IDENTITY_INSERT [dbo].[PTH_Samples] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_SampleTests] ON 

INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, 1, 1, 1, N'Completed', 3, CAST(N'2026-08-20T09:30:00.0000000' AS DateTime2), CAST(N'2026-08-20T10:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.3333333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, 1, 2, 2, N'Completed', 3, CAST(N'2026-08-20T09:30:00.0000000' AS DateTime2), CAST(N'2026-08-20T10:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.3333333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, 2, 3, 1, N'Completed', 3, CAST(N'2026-08-21T10:00:00.0000000' AS DateTime2), CAST(N'2026-08-21T11:00:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.3766667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, 4, 4, 2, N'In-Progress', 3, CAST(N'2026-08-22T11:30:00.0000000' AS DateTime2), NULL, CAST(N'2026-08-22T10:02:41.4133333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, 5, 5, 2, N'Completed', 3, CAST(N'2026-08-19T08:00:00.0000000' AS DateTime2), CAST(N'2026-08-19T09:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.4366667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, 6, 6, 3, N'Completed', 3, CAST(N'2026-08-19T08:00:00.0000000' AS DateTime2), CAST(N'2026-08-19T09:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.4366667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, 7, 8, 1, N'Collected', NULL, NULL, NULL, CAST(N'2026-08-22T10:18:04.7416793' AS DateTime2), N'System', CAST(N'2026-08-22T10:21:32.0104315' AS DateTime2), N'System', 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (8, 7, 9, 2, N'Collected', NULL, NULL, NULL, CAST(N'2026-08-22T10:18:04.7752694' AS DateTime2), N'System', CAST(N'2026-08-22T10:21:32.0104317' AS DateTime2), N'System', 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (9, 8, 10, 3, N'Collected', NULL, NULL, NULL, CAST(N'2026-08-22T10:18:04.7754623' AS DateTime2), N'System', CAST(N'2026-08-22T10:21:29.0224055' AS DateTime2), N'System', 0)
INSERT [dbo].[PTH_SampleTests] ([SampleTestId], [SampleId], [OrderDetailId], [TestId], [Status], [AssignedToId], [StartedOn], [CompletedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (10, 9, 11, 4, N'Collected', NULL, NULL, NULL, CAST(N'2026-08-22T10:18:04.7755494' AS DateTime2), N'System', CAST(N'2026-08-22T10:19:56.1577895' AS DateTime2), N'System', 0)
SET IDENTITY_INSERT [dbo].[PTH_SampleTests] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_SampleTypes] ON 

INSERT [dbo].[PTH_SampleTypes] ([SampleTypeId], [SampleTypeCode], [SampleTypeName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'BLD', N'Whole Blood', N'EDTA Vials (Purple-top)', 1, CAST(N'2026-08-22T10:02:41.2200000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_SampleTypes] ([SampleTypeId], [SampleTypeCode], [SampleTypeName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'URN', N'Urine Specimen', N'Sterile Urine Container', 1, CAST(N'2026-08-22T10:02:41.2200000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_SampleTypes] ([SampleTypeId], [SampleTypeCode], [SampleTypeName], [Description], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'SER', N'Serum', N'Clot Activator Vials (Red-top)', 1, CAST(N'2026-08-22T10:02:41.2200000' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_SampleTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_TestPackages] ON 

INSERT [dbo].[PTH_TestPackages] ([PackageId], [PackageCode], [PackageName], [Description], [OriginalPrice], [PackagePrice], [DiscountAmount], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'WEL-CHK', N'General Wellness Package', N'Includes CBC, FBS, and LFT profiles at bundled savings.', CAST(1250.00 AS Decimal(18, 2)), CAST(999.00 AS Decimal(18, 2)), CAST(251.00 AS Decimal(18, 2)), 1, CAST(N'2026-08-22T10:02:41.2700000' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_TestPackages] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_TestParameters] ON 

INSERT [dbo].[PTH_TestParameters] ([TestParameterId], [TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, 1, N'HB', N'Hemoglobin', 1, N'Numeric', N'g/dL', CAST(13.0000 AS Decimal(18, 4)), CAST(17.0000 AS Decimal(18, 4)), CAST(12.0000 AS Decimal(18, 4)), CAST(15.0000 AS Decimal(18, 4)), CAST(11.0000 AS Decimal(18, 4)), CAST(14.0000 AS Decimal(18, 4)), CAST(7.0000 AS Decimal(18, 4)), CAST(20.0000 AS Decimal(18, 4)), N'12.0 - 17.0', CAST(N'2026-08-22T10:02:41.2433333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestParameters] ([TestParameterId], [TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, 1, N'WBC', N'WBC Count', 2, N'Numeric', N'/µL', CAST(4000.0000 AS Decimal(18, 4)), CAST(11000.0000 AS Decimal(18, 4)), CAST(4000.0000 AS Decimal(18, 4)), CAST(11000.0000 AS Decimal(18, 4)), CAST(5000.0000 AS Decimal(18, 4)), CAST(13000.0000 AS Decimal(18, 4)), CAST(2000.0000 AS Decimal(18, 4)), CAST(30000.0000 AS Decimal(18, 4)), N'4000 - 11000', CAST(N'2026-08-22T10:02:41.2433333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestParameters] ([TestParameterId], [TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, 1, N'PLT', N'Platelet Count', 3, N'Numeric', N'lakh/µL', CAST(1.5000 AS Decimal(18, 4)), CAST(4.5000 AS Decimal(18, 4)), CAST(1.5000 AS Decimal(18, 4)), CAST(4.5000 AS Decimal(18, 4)), CAST(1.5000 AS Decimal(18, 4)), CAST(4.5000 AS Decimal(18, 4)), CAST(0.5000 AS Decimal(18, 4)), CAST(10.0000 AS Decimal(18, 4)), N'1.5 - 4.5', CAST(N'2026-08-22T10:02:41.2433333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestParameters] ([TestParameterId], [TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, 2, N'SUG_F', N'Fasting Sugar', 1, N'Numeric', N'mg/dL', CAST(70.0000 AS Decimal(18, 4)), CAST(100.0000 AS Decimal(18, 4)), CAST(70.0000 AS Decimal(18, 4)), CAST(100.0000 AS Decimal(18, 4)), CAST(70.0000 AS Decimal(18, 4)), CAST(100.0000 AS Decimal(18, 4)), CAST(50.0000 AS Decimal(18, 4)), CAST(350.0000 AS Decimal(18, 4)), N'70 - 100', CAST(N'2026-08-22T10:02:41.2500000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestParameters] ([TestParameterId], [TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, 3, N'BIL_T', N'Bilirubin Total', 1, N'Numeric', N'mg/dL', CAST(0.1000 AS Decimal(18, 4)), CAST(1.2000 AS Decimal(18, 4)), CAST(0.1000 AS Decimal(18, 4)), CAST(1.2000 AS Decimal(18, 4)), CAST(0.1000 AS Decimal(18, 4)), CAST(1.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(5.0000 AS Decimal(18, 4)), N'0.1 - 1.2', CAST(N'2026-08-22T10:02:41.2566667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestParameters] ([TestParameterId], [TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, 3, N'SGPT', N'SGPT (ALT)', 2, N'Numeric', N'U/L', CAST(5.0000 AS Decimal(18, 4)), CAST(50.0000 AS Decimal(18, 4)), CAST(5.0000 AS Decimal(18, 4)), CAST(35.0000 AS Decimal(18, 4)), CAST(5.0000 AS Decimal(18, 4)), CAST(40.0000 AS Decimal(18, 4)), CAST(0.0000 AS Decimal(18, 4)), CAST(500.0000 AS Decimal(18, 4)), N'5 - 50', CAST(N'2026-08-22T10:02:41.2566667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestParameters] ([TestParameterId], [TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, 4, N'URN_COL', N'Color', 1, N'Text', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Pale Yellow', CAST(N'2026-08-22T10:02:41.2600000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestParameters] ([TestParameterId], [TestId], [ParameterCode], [ParameterName], [DisplayOrder], [ResultType], [Unit], [MaleMin], [MaleMax], [FemaleMin], [FemaleMax], [ChildMin], [ChildMax], [CriticalLow], [CriticalHigh], [DefaultReferenceRange], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (8, 4, N'URN_SUG', N'Sugar', 2, N'Text', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Nil', CAST(N'2026-08-22T10:02:41.2600000' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_TestParameters] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_TestResults] ON 

INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, 1, 1, N'14.5', CAST(14.5000 AS Decimal(18, 4)), N'Normal', N'13.0 - 17.0', N'g/dL', NULL, 3, CAST(N'2026-08-20T10:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.3466667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, 1, 2, N'5.5', CAST(5.5000 AS Decimal(18, 4)), N'Normal', N'4.0 - 11.0', N'/µL', NULL, 3, CAST(N'2026-08-20T10:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.3466667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, 1, 3, N'2.8', CAST(2.8000 AS Decimal(18, 4)), N'Normal', N'1.5 - 4.5', N'lakh/µL', NULL, 3, CAST(N'2026-08-20T10:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.3466667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, 2, 4, N'95.0', CAST(95.0000 AS Decimal(18, 4)), N'Normal', N'70 - 100', N'mg/dL', NULL, 3, CAST(N'2026-08-20T10:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-20T11:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.3466667' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, 3, 1, N'9.2', CAST(9.2000 AS Decimal(18, 4)), N'Low', N'12.0 - 15.0', N'g/dL', NULL, 3, CAST(N'2026-08-21T11:00:00.0000000' AS DateTime2), NULL, NULL, CAST(N'2026-08-22T10:02:41.3833333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, 3, 2, N'6.1', CAST(6.1000 AS Decimal(18, 4)), N'Normal', N'4.0 - 11.0', N'/µL', NULL, 3, CAST(N'2026-08-21T11:00:00.0000000' AS DateTime2), NULL, NULL, CAST(N'2026-08-22T10:02:41.3833333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (7, 3, 3, N'2.1', CAST(2.1000 AS Decimal(18, 4)), N'Normal', N'1.5 - 4.5', N'lakh/µL', NULL, 3, CAST(N'2026-08-21T11:00:00.0000000' AS DateTime2), NULL, NULL, CAST(N'2026-08-22T10:02:41.3833333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (8, 5, 4, N'362.0', CAST(362.0000 AS Decimal(18, 4)), N'Critical', N'70 - 100', N'mg/dL', NULL, 3, CAST(N'2026-08-19T09:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-19T10:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.4433333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (9, 6, 5, N'4.2', CAST(4.2000 AS Decimal(18, 4)), N'High', N'0.1 - 1.2', N'mg/dL', NULL, 3, CAST(N'2026-08-19T09:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-19T10:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.4433333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_TestResults] ([TestResultId], [SampleTestId], [TestParameterId], [ResultValue], [ResultNumericValue], [ResultStatus], [ReferenceRange], [Unit], [Remarks], [EnteredById], [EnteredOn], [VerifiedById], [VerifiedOn], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (10, 6, 6, N'185.0', CAST(185.0000 AS Decimal(18, 4)), N'High', N'5 - 50', N'U/L', NULL, 3, CAST(N'2026-08-19T09:30:00.0000000' AS DateTime2), 4, CAST(N'2026-08-19T10:30:00.0000000' AS DateTime2), CAST(N'2026-08-22T10:02:41.4433333' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_TestResults] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Tests] ON 

INSERT [dbo].[PTH_Tests] ([TestId], [TestCode], [TestName], [DepartmentId], [SampleTypeId], [TestType], [Description], [Price], [TATMinutes], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'CBC', N'Complete Blood Count', 1, 1, N'Profile', NULL, CAST(350.00 AS Decimal(18, 2)), 120, 1, CAST(N'2026-08-22T10:02:41.2333333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Tests] ([TestId], [TestCode], [TestName], [DepartmentId], [SampleTypeId], [TestType], [Description], [Price], [TATMinutes], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'FBS', N'Blood Sugar Fasting', 2, 1, N'Single', NULL, CAST(150.00 AS Decimal(18, 2)), 60, 1, CAST(N'2026-08-22T10:02:41.2333333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Tests] ([TestId], [TestCode], [TestName], [DepartmentId], [SampleTypeId], [TestType], [Description], [Price], [TATMinutes], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'LFT', N'Liver Function Test', 2, 3, N'Profile', NULL, CAST(750.00 AS Decimal(18, 2)), 180, 1, CAST(N'2026-08-22T10:02:41.2333333' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Tests] ([TestId], [TestCode], [TestName], [DepartmentId], [SampleTypeId], [TestType], [Description], [Price], [TATMinutes], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, N'URN', N'Urine Routine', 3, 2, N'Profile', NULL, CAST(200.00 AS Decimal(18, 2)), 90, 1, CAST(N'2026-08-22T10:02:41.2333333' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Tests] OFF
GO
SET IDENTITY_INSERT [dbo].[PTH_Users] ON 

INSERT [dbo].[PTH_Users] ([UserId], [Username], [PasswordHash], [FullName], [Email], [Mobile], [RoleId], [LastLoginDate], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (1, N'admin', N'e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7', N'System Admin', N'admin@patholab.com', N'9876543210', 1, CAST(N'2026-08-22T10:13:15.2976728' AS DateTime2), 1, CAST(N'2026-08-22T10:02:41.2100000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Users] ([UserId], [Username], [PasswordHash], [FullName], [Email], [Mobile], [RoleId], [LastLoginDate], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (2, N'reception', N'238f1cf33d39690fba3c171984fc1120a1181d236723d337e6a2fdc8d92ae88a', N'Rita Sharma', N'reception@patholab.com', N'9876543211', 3, NULL, 1, CAST(N'2026-08-22T10:02:41.2100000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Users] ([UserId], [Username], [PasswordHash], [FullName], [Email], [Mobile], [RoleId], [LastLoginDate], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (3, N'technician', N'7cd5fea080d2a3f7de469cca064541b414698acd4173e0d4823d03351d12a24e', N'Tushar Roy', N'technician@patholab.com', N'9876543212', 4, NULL, 1, CAST(N'2026-08-22T10:02:41.2100000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Users] ([UserId], [Username], [PasswordHash], [FullName], [Email], [Mobile], [RoleId], [LastLoginDate], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (4, N'pathologist', N'51d38824098a6cc0ed0d552ed38a7174ce0d3f53483ed4e00bb7c8e3b832ed5b', N'Dr. Priya Nair', N'pathologist@patholab.com', N'9876543213', 5, NULL, 1, CAST(N'2026-08-22T10:02:41.2100000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Users] ([UserId], [Username], [PasswordHash], [FullName], [Email], [Mobile], [RoleId], [LastLoginDate], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (5, N'accountant', N'381d8632ee588cb77a06f0da28d173fbc17abd815133ce19c4959f74bc19eeb0', N'Anil Mehta', N'accountant@patholab.com', N'9876543214', 6, NULL, 1, CAST(N'2026-08-22T10:02:41.2100000' AS DateTime2), N'System', NULL, NULL, 0)
INSERT [dbo].[PTH_Users] ([UserId], [Username], [PasswordHash], [FullName], [Email], [Mobile], [RoleId], [LastLoginDate], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [DeletedFlag]) VALUES (6, N'agent', N'c3206ca954a4953c1c1b82b4167bd097deab0e228207e3b3296237e369f39d34', N'Amit Verma', N'agent@patholab.com', N'9876543215', 7, NULL, 1, CAST(N'2026-08-22T10:02:41.2100000' AS DateTime2), N'System', NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PTH_Users] OFF
GO
ALTER TABLE [dbo].[PTH_AuditLogs] ADD  CONSTRAINT [DF_PTH_AuditLogs_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Departments] ADD  CONSTRAINT [DF_PTH_Departments_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_Departments] ADD  CONSTRAINT [DF_PTH_Departments_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Departments] ADD  CONSTRAINT [DF_PTH_Departments_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_Departments] ADD  CONSTRAINT [DF_PTH_Departments_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Doctors] ADD  CONSTRAINT [DF_PTH_Doctors_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_Doctors] ADD  CONSTRAINT [DF_PTH_Doctors_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Doctors] ADD  CONSTRAINT [DF_PTH_Doctors_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_Doctors] ADD  CONSTRAINT [DF_PTH_Doctors_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_HomeCollections] ADD  CONSTRAINT [DF_PTH_HomeCollections_Status]  DEFAULT ('Requested') FOR [Status]
GO
ALTER TABLE [dbo].[PTH_HomeCollections] ADD  CONSTRAINT [DF_PTH_HomeCollections_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Invoices] ADD  CONSTRAINT [DF_PTH_Invoices_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Notifications] ADD  CONSTRAINT [DF_PTH_Notifications_Status]  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[PTH_Notifications] ADD  CONSTRAINT [DF_PTH_Notifications_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Notifications] ADD  CONSTRAINT [DF_PTH_Notifications_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_Notifications] ADD  CONSTRAINT [DF_PTH_Notifications_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_OrderDetails] ADD  CONSTRAINT [DF_PTH_OrderDetails_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_OrderDetails] ADD  CONSTRAINT [DF_PTH_OrderDetails_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_OrderDetails] ADD  CONSTRAINT [DF_PTH_OrderDetails_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Orders] ADD  CONSTRAINT [DF_PTH_Orders_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_PackageTests] ADD  CONSTRAINT [DF_PTH_PackageTests_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_PackageTests] ADD  CONSTRAINT [DF_PTH_PackageTests_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_PackageTests] ADD  CONSTRAINT [DF_PTH_PackageTests_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Patients] ADD  CONSTRAINT [DF_PTH_Patients_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_Patients] ADD  CONSTRAINT [DF_PTH_Patients_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Patients] ADD  CONSTRAINT [DF_PTH_Patients_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_Patients] ADD  CONSTRAINT [DF_PTH_Patients_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Payments] ADD  CONSTRAINT [DF_PTH_Payments_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Permissions] ADD  CONSTRAINT [DF_PTH_Permissions_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_Permissions] ADD  CONSTRAINT [DF_PTH_Permissions_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Permissions] ADD  CONSTRAINT [DF_PTH_Permissions_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_Permissions] ADD  CONSTRAINT [DF_PTH_Permissions_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_ReportDetails] ADD  CONSTRAINT [DF_PTH_ReportDetails_DisplayOrder]  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[PTH_ReportDetails] ADD  CONSTRAINT [DF_PTH_ReportDetails_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_ReportDetails] ADD  CONSTRAINT [DF_PTH_ReportDetails_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_ReportDetails] ADD  CONSTRAINT [DF_PTH_ReportDetails_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Reports] ADD  CONSTRAINT [DF_PTH_Reports_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_RolePermissions] ADD  CONSTRAINT [DF_PTH_RolePermissions_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_RolePermissions] ADD  CONSTRAINT [DF_PTH_RolePermissions_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_RolePermissions] ADD  CONSTRAINT [DF_PTH_RolePermissions_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Roles] ADD  CONSTRAINT [DF_PTH_Roles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_Roles] ADD  CONSTRAINT [DF_PTH_Roles_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Roles] ADD  CONSTRAINT [DF_PTH_Roles_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_Roles] ADD  CONSTRAINT [DF_PTH_Roles_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Samples] ADD  CONSTRAINT [DF_PTH_Samples_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_SampleTests] ADD  CONSTRAINT [DF_PTH_SampleTests_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_SampleTests] ADD  CONSTRAINT [DF_PTH_SampleTests_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_SampleTests] ADD  CONSTRAINT [DF_PTH_SampleTests_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_SampleTypes] ADD  CONSTRAINT [DF_PTH_SampleTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_SampleTypes] ADD  CONSTRAINT [DF_PTH_SampleTypes_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_SampleTypes] ADD  CONSTRAINT [DF_PTH_SampleTypes_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_SampleTypes] ADD  CONSTRAINT [DF_PTH_SampleTypes_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_TestPackages] ADD  CONSTRAINT [DF_PTH_TestPackages_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_TestPackages] ADD  CONSTRAINT [DF_PTH_TestPackages_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_TestPackages] ADD  CONSTRAINT [DF_PTH_TestPackages_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_TestPackages] ADD  CONSTRAINT [DF_PTH_TestPackages_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_TestParameters] ADD  CONSTRAINT [DF_PTH_TestParameters_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_TestParameters] ADD  CONSTRAINT [DF_PTH_TestParameters_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_TestParameters] ADD  CONSTRAINT [DF_PTH_TestParameters_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_TestResults] ADD  CONSTRAINT [DF_PTH_TestResults_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_TestResults] ADD  CONSTRAINT [DF_PTH_TestResults_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_TestResults] ADD  CONSTRAINT [DF_PTH_TestResults_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Tests] ADD  CONSTRAINT [DF_PTH_Tests_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_Tests] ADD  CONSTRAINT [DF_PTH_Tests_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Tests] ADD  CONSTRAINT [DF_PTH_Tests_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_Tests] ADD  CONSTRAINT [DF_PTH_Tests_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_Users] ADD  CONSTRAINT [DF_PTH_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PTH_Users] ADD  CONSTRAINT [DF_PTH_Users_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PTH_Users] ADD  CONSTRAINT [DF_PTH_Users_CreatedBy]  DEFAULT ('System') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[PTH_Users] ADD  CONSTRAINT [DF_PTH_Users_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[PTH_AuditLogs]  WITH CHECK ADD  CONSTRAINT [FK_PTH_AuditLogs_PTH_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_AuditLogs] CHECK CONSTRAINT [FK_PTH_AuditLogs_PTH_Users_UserId]
GO
ALTER TABLE [dbo].[PTH_HomeCollections]  WITH CHECK ADD  CONSTRAINT [FK_PTH_HomeCollections_PTH_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[PTH_Orders] ([OrderId])
GO
ALTER TABLE [dbo].[PTH_HomeCollections] CHECK CONSTRAINT [FK_PTH_HomeCollections_PTH_Orders_OrderId]
GO
ALTER TABLE [dbo].[PTH_HomeCollections]  WITH CHECK ADD  CONSTRAINT [FK_PTH_HomeCollections_PTH_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[PTH_Patients] ([PatientId])
GO
ALTER TABLE [dbo].[PTH_HomeCollections] CHECK CONSTRAINT [FK_PTH_HomeCollections_PTH_Patients_PatientId]
GO
ALTER TABLE [dbo].[PTH_HomeCollections]  WITH CHECK ADD  CONSTRAINT [FK_PTH_HomeCollections_PTH_Users_AssignedAgentId] FOREIGN KEY([AssignedAgentId])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_HomeCollections] CHECK CONSTRAINT [FK_PTH_HomeCollections_PTH_Users_AssignedAgentId]
GO
ALTER TABLE [dbo].[PTH_Invoices]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Invoices_PTH_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[PTH_Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_Invoices] CHECK CONSTRAINT [FK_PTH_Invoices_PTH_Orders_OrderId]
GO
ALTER TABLE [dbo].[PTH_Invoices]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Invoices_PTH_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[PTH_Patients] ([PatientId])
GO
ALTER TABLE [dbo].[PTH_Invoices] CHECK CONSTRAINT [FK_PTH_Invoices_PTH_Patients_PatientId]
GO
ALTER TABLE [dbo].[PTH_Notifications]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Notifications_PTH_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[PTH_Patients] ([PatientId])
GO
ALTER TABLE [dbo].[PTH_Notifications] CHECK CONSTRAINT [FK_PTH_Notifications_PTH_Patients_PatientId]
GO
ALTER TABLE [dbo].[PTH_OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_PTH_OrderDetails_PTH_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[PTH_Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_OrderDetails] CHECK CONSTRAINT [FK_PTH_OrderDetails_PTH_Orders_OrderId]
GO
ALTER TABLE [dbo].[PTH_OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_PTH_OrderDetails_PTH_TestPackages_PackageId] FOREIGN KEY([PackageId])
REFERENCES [dbo].[PTH_TestPackages] ([PackageId])
GO
ALTER TABLE [dbo].[PTH_OrderDetails] CHECK CONSTRAINT [FK_PTH_OrderDetails_PTH_TestPackages_PackageId]
GO
ALTER TABLE [dbo].[PTH_OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_PTH_OrderDetails_PTH_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[PTH_Tests] ([TestId])
GO
ALTER TABLE [dbo].[PTH_OrderDetails] CHECK CONSTRAINT [FK_PTH_OrderDetails_PTH_Tests_TestId]
GO
ALTER TABLE [dbo].[PTH_Orders]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Orders_PTH_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[PTH_Patients] ([PatientId])
GO
ALTER TABLE [dbo].[PTH_Orders] CHECK CONSTRAINT [FK_PTH_Orders_PTH_Patients_PatientId]
GO
ALTER TABLE [dbo].[PTH_PackageTests]  WITH CHECK ADD  CONSTRAINT [FK_PTH_PackageTests_PTH_TestPackages_PackageId] FOREIGN KEY([PackageId])
REFERENCES [dbo].[PTH_TestPackages] ([PackageId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_PackageTests] CHECK CONSTRAINT [FK_PTH_PackageTests_PTH_TestPackages_PackageId]
GO
ALTER TABLE [dbo].[PTH_PackageTests]  WITH CHECK ADD  CONSTRAINT [FK_PTH_PackageTests_PTH_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[PTH_Tests] ([TestId])
GO
ALTER TABLE [dbo].[PTH_PackageTests] CHECK CONSTRAINT [FK_PTH_PackageTests_PTH_Tests_TestId]
GO
ALTER TABLE [dbo].[PTH_Payments]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Payments_PTH_Invoices_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[PTH_Invoices] ([InvoiceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_Payments] CHECK CONSTRAINT [FK_PTH_Payments_PTH_Invoices_InvoiceId]
GO
ALTER TABLE [dbo].[PTH_ReportDetails]  WITH CHECK ADD  CONSTRAINT [FK_PTH_ReportDetails_PTH_Reports_ReportId] FOREIGN KEY([ReportId])
REFERENCES [dbo].[PTH_Reports] ([ReportId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_ReportDetails] CHECK CONSTRAINT [FK_PTH_ReportDetails_PTH_Reports_ReportId]
GO
ALTER TABLE [dbo].[PTH_ReportDetails]  WITH CHECK ADD  CONSTRAINT [FK_PTH_ReportDetails_PTH_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[PTH_Tests] ([TestId])
GO
ALTER TABLE [dbo].[PTH_ReportDetails] CHECK CONSTRAINT [FK_PTH_ReportDetails_PTH_Tests_TestId]
GO
ALTER TABLE [dbo].[PTH_Reports]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Reports_PTH_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[PTH_Orders] ([OrderId])
GO
ALTER TABLE [dbo].[PTH_Reports] CHECK CONSTRAINT [FK_PTH_Reports_PTH_Orders_OrderId]
GO
ALTER TABLE [dbo].[PTH_Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_PTH_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[PTH_Patients] ([PatientId])
GO
ALTER TABLE [dbo].[PTH_Reports] CHECK CONSTRAINT [FK_Reports_PTH_Patients_PatientId]
GO
ALTER TABLE [dbo].[PTH_Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_PTH_Users_VerifiedById] FOREIGN KEY([VerifiedById])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_Reports] CHECK CONSTRAINT [FK_Reports_PTH_Users_VerifiedById]
GO
ALTER TABLE [dbo].[PTH_RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_PTH_RolePermissions_PTH_Permissions_PermissionId] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[PTH_Permissions] ([PermissionId])
GO
ALTER TABLE [dbo].[PTH_RolePermissions] CHECK CONSTRAINT [FK_PTH_RolePermissions_PTH_Permissions_PermissionId]
GO
ALTER TABLE [dbo].[PTH_RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_PTH_RolePermissions_PTH_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[PTH_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[PTH_RolePermissions] CHECK CONSTRAINT [FK_PTH_RolePermissions_PTH_Roles_RoleId]
GO
ALTER TABLE [dbo].[PTH_Samples]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Samples_PTH_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[PTH_Orders] ([OrderId])
GO
ALTER TABLE [dbo].[PTH_Samples] CHECK CONSTRAINT [FK_PTH_Samples_PTH_Orders_OrderId]
GO
ALTER TABLE [dbo].[PTH_Samples]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Samples_PTH_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[PTH_Patients] ([PatientId])
GO
ALTER TABLE [dbo].[PTH_Samples] CHECK CONSTRAINT [FK_PTH_Samples_PTH_Patients_PatientId]
GO
ALTER TABLE [dbo].[PTH_Samples]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Samples_PTH_SampleTypes_SampleTypeId] FOREIGN KEY([SampleTypeId])
REFERENCES [dbo].[PTH_SampleTypes] ([SampleTypeId])
GO
ALTER TABLE [dbo].[PTH_Samples] CHECK CONSTRAINT [FK_PTH_Samples_PTH_SampleTypes_SampleTypeId]
GO
ALTER TABLE [dbo].[PTH_Samples]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Samples_PTH_Users_CollectedById] FOREIGN KEY([CollectedById])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_Samples] CHECK CONSTRAINT [FK_PTH_Samples_PTH_Users_CollectedById]
GO
ALTER TABLE [dbo].[PTH_Samples]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Samples_PTH_Users_ReceivedById] FOREIGN KEY([ReceivedById])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_Samples] CHECK CONSTRAINT [FK_PTH_Samples_PTH_Users_ReceivedById]
GO
ALTER TABLE [dbo].[PTH_SampleTests]  WITH CHECK ADD  CONSTRAINT [FK_PTH_SampleTests_PTH_OrderDetails_OrderDetailId] FOREIGN KEY([OrderDetailId])
REFERENCES [dbo].[PTH_OrderDetails] ([OrderDetailId])
GO
ALTER TABLE [dbo].[PTH_SampleTests] CHECK CONSTRAINT [FK_PTH_SampleTests_PTH_OrderDetails_OrderDetailId]
GO
ALTER TABLE [dbo].[PTH_SampleTests]  WITH CHECK ADD  CONSTRAINT [FK_PTH_SampleTests_PTH_Samples_SampleId] FOREIGN KEY([SampleId])
REFERENCES [dbo].[PTH_Samples] ([SampleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_SampleTests] CHECK CONSTRAINT [FK_PTH_SampleTests_PTH_Samples_SampleId]
GO
ALTER TABLE [dbo].[PTH_SampleTests]  WITH CHECK ADD  CONSTRAINT [FK_PTH_SampleTests_PTH_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[PTH_Tests] ([TestId])
GO
ALTER TABLE [dbo].[PTH_SampleTests] CHECK CONSTRAINT [FK_PTH_SampleTests_PTH_Tests_TestId]
GO
ALTER TABLE [dbo].[PTH_SampleTests]  WITH CHECK ADD  CONSTRAINT [FK_PTH_SampleTests_PTH_Users_AssignedToId] FOREIGN KEY([AssignedToId])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_SampleTests] CHECK CONSTRAINT [FK_PTH_SampleTests_PTH_Users_AssignedToId]
GO
ALTER TABLE [dbo].[PTH_TestParameters]  WITH CHECK ADD  CONSTRAINT [FK_PTH_TestParameters_PTH_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[PTH_Tests] ([TestId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_TestParameters] CHECK CONSTRAINT [FK_PTH_TestParameters_PTH_Tests_TestId]
GO
ALTER TABLE [dbo].[PTH_TestResultHistory]  WITH CHECK ADD  CONSTRAINT [FK_PTH_TestResultHistory_PTH_TestResults_TestResultId] FOREIGN KEY([TestResultId])
REFERENCES [dbo].[PTH_TestResults] ([TestResultId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_TestResultHistory] CHECK CONSTRAINT [FK_PTH_TestResultHistory_PTH_TestResults_TestResultId]
GO
ALTER TABLE [dbo].[PTH_TestResultHistory]  WITH CHECK ADD  CONSTRAINT [FK_PTH_TestResultHistory_PTH_Users_ChangedById] FOREIGN KEY([ChangedById])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_TestResultHistory] CHECK CONSTRAINT [FK_PTH_TestResultHistory_PTH_Users_ChangedById]
GO
ALTER TABLE [dbo].[PTH_TestResults]  WITH CHECK ADD  CONSTRAINT [FK_PTH_TestResults_PTH_SampleTests_SampleTestId] FOREIGN KEY([SampleTestId])
REFERENCES [dbo].[PTH_SampleTests] ([SampleTestId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PTH_TestResults] CHECK CONSTRAINT [FK_PTH_TestResults_PTH_SampleTests_SampleTestId]
GO
ALTER TABLE [dbo].[PTH_TestResults]  WITH CHECK ADD  CONSTRAINT [FK_PTH_TestResults_PTH_TestParameters_TestParameterId] FOREIGN KEY([TestParameterId])
REFERENCES [dbo].[PTH_TestParameters] ([TestParameterId])
GO
ALTER TABLE [dbo].[PTH_TestResults] CHECK CONSTRAINT [FK_PTH_TestResults_PTH_TestParameters_TestParameterId]
GO
ALTER TABLE [dbo].[PTH_TestResults]  WITH CHECK ADD  CONSTRAINT [FK_PTH_TestResults_PTH_Users_EnteredById] FOREIGN KEY([EnteredById])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_TestResults] CHECK CONSTRAINT [FK_PTH_TestResults_PTH_Users_EnteredById]
GO
ALTER TABLE [dbo].[PTH_TestResults]  WITH CHECK ADD  CONSTRAINT [FK_PTH_TestResults_PTH_Users_VerifiedById] FOREIGN KEY([VerifiedById])
REFERENCES [dbo].[PTH_Users] ([UserId])
GO
ALTER TABLE [dbo].[PTH_TestResults] CHECK CONSTRAINT [FK_PTH_TestResults_PTH_Users_VerifiedById]
GO
ALTER TABLE [dbo].[PTH_Tests]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Tests_PTH_Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[PTH_Departments] ([DepartmentId])
GO
ALTER TABLE [dbo].[PTH_Tests] CHECK CONSTRAINT [FK_PTH_Tests_PTH_Departments_DepartmentId]
GO
ALTER TABLE [dbo].[PTH_Tests]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Tests_PTH_SampleTypes_SampleTypeId] FOREIGN KEY([SampleTypeId])
REFERENCES [dbo].[PTH_SampleTypes] ([SampleTypeId])
GO
ALTER TABLE [dbo].[PTH_Tests] CHECK CONSTRAINT [FK_PTH_Tests_PTH_SampleTypes_SampleTypeId]
GO
ALTER TABLE [dbo].[PTH_Users]  WITH CHECK ADD  CONSTRAINT [FK_PTH_Users_PTH_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[PTH_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[PTH_Users] CHECK CONSTRAINT [FK_PTH_Users_PTH_Roles_RoleId]
GO
