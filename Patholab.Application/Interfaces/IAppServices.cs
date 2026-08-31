using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Patholab.Domain.Enums;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task ChangePasswordAsync(string username, ChangePasswordRequest request, CancellationToken cancellationToken = default);
    }

    public interface IPatientService
    {
        Task<PatientDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<PatientDto>> GetAllPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
        Task<PatientDto> CreateAsync(PatientDto dto, CancellationToken cancellationToken = default);
        Task<PatientDto> UpdateAsync(int id, PatientDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }

    public interface IDoctorService
    {
        Task<DoctorDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<DoctorDto>> GetAllPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
        Task<DoctorDto> CreateAsync(DoctorDto dto, CancellationToken cancellationToken = default);
        Task<DoctorDto> UpdateAsync(int id, DoctorDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }

    public interface ITestService
    {
        Task<TestDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<TestDto>> GetAllPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
        Task<TestDto> CreateAsync(TestDto dto, CancellationToken cancellationToken = default);
        Task<TestDto> UpdateAsync(int id, TestDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        
        Task<List<DepartmentDto>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
        Task<List<SampleTypeDto>> GetSampleTypesAsync(CancellationToken cancellationToken = default);
        Task<List<TestPackageDto>> GetPackagesAsync(CancellationToken cancellationToken = default);
    }

    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
        Task<OrderDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<OrderDto> GetOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<PagedResult<OrderDto>> GetOrdersPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
        Task CancelOrderAsync(int orderId, string remarks, CancellationToken cancellationToken = default);
    }

    public interface ISampleService
    {
        Task<SampleDto> GetSampleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<SampleDto>> GetSamplesPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
        Task<List<SampleDto>> GetPendingSamplesAsync(CancellationToken cancellationToken = default);
        Task CollectSampleAsync(int orderDetailId, string barcode, int collectedById, CancellationToken cancellationToken = default);
        Task ReceiveSampleAsync(int sampleId, int receivedById, CancellationToken cancellationToken = default);
        Task RejectSampleAsync(int sampleId, string reason, string? remarks, int rejectedById, CancellationToken cancellationToken = default);
    }

    public interface ITestResultService
    {
        Task EnterResultsAsync(EnterResultsRequest request, int enteredById, CancellationToken cancellationToken = default);
        Task<List<TestResultDto>> GetResultsBySampleIdAsync(int sampleId, CancellationToken cancellationToken = default);
        Task SubmitResultsForVerificationAsync(int sampleId, CancellationToken cancellationToken = default);
        Task<List<TestResultHistoryDto>> GetHistoryAsync(int resultId, CancellationToken cancellationToken = default);
    }

    public interface IReportService
    {
        Task<ReportDto> GetReportByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ReportDto> GetReportByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
        Task<ReportDto> VerifyReportAsync(int reportId, int verifiedById, string? remarks, CancellationToken cancellationToken = default);
        Task<ReportDto> PublishReportAsync(int reportId, int publishedById, CancellationToken cancellationToken = default);
        Task<ReportVerificationResponse> VerifyPublicReportAsync(string reportNumber, CancellationToken cancellationToken = default);
        Task<byte[]> DownloadReportPdfAsync(int reportId, CancellationToken cancellationToken = default);
        Task<PagedResult<ReportDto>> GetReportsPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
    }

    public interface IBillingService
    {
        Task<InvoiceDto> GetInvoiceByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<InvoiceDto>> GetInvoicesPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
        Task<PaymentDto> CollectPaymentAsync(CreatePaymentRequest request, string receivedBy, CancellationToken cancellationToken = default);
        Task<PagedResult<PaymentDto>> GetPaymentsPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
        Task UpdateInvoiceDiscountAsync(int invoiceId, decimal discountAmount, CancellationToken cancellationToken = default);
    }

    public interface IHomeCollectionService
    {
        Task<HomeCollectionDto> RequestCollectionAsync(RequestHomeCollectionRequest request, CancellationToken cancellationToken = default);
        Task AssignAgentAsync(int hcId, int agentId, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(int hcId, HomeCollectionStatus status, string? remarks, CancellationToken cancellationToken = default);
        Task<PagedResult<HomeCollectionDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
    }

    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
        Task<List<RevenueChartItem>> GetRevenueChartDataAsync(int days, CancellationToken cancellationToken = default);
        Task<List<TestStatsDto>> GetTestStatsAsync(int limit, CancellationToken cancellationToken = default);
        Task<List<DepartmentStatsDto>> GetDepartmentStatsAsync(CancellationToken cancellationToken = default);
    }

    public interface IAuditLogService
    {
        Task<PagedResult<AuditLogDto>> GetAuditLogsPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
        Task LogActionAsync(int? userId, string module, string action, string table, int recordId, string? oldVal, string? newVal, string? ip, string? ua, CancellationToken cancellationToken = default);
    }
}
