using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Patholab.Domain.Enums;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.MVC.Services
{
    public class PatholabApiClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatholabApiClient(HttpClient client, IHttpContextAccessor httpContextAccessor, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _client = client;
            var baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7198/";
            _client.BaseAddress = new Uri(baseUrl);
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthHeader()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var token = session.GetString("JWToken");
                if (!string.IsNullOrEmpty(token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            // Append target API endpoint URI to the MVC response headers so developer can see it in network tab
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && response.RequestMessage != null)
            {
                var apiUri = response.RequestMessage.RequestUri?.ToString();
                var method = response.RequestMessage.Method.Method;
                if (!string.IsNullOrEmpty(apiUri))
                {
                    var headerKey = "X-API-Call";
                    var headerVal = $"{method} {apiUri}";
                    if (!httpContext.Response.Headers.ContainsKey(headerKey))
                    {
                        httpContext.Response.Headers.Add(headerKey, headerVal);
                    }
                    else
                    {
                        httpContext.Response.Headers[headerKey] = string.Concat(httpContext.Response.Headers[headerKey], ", ", headerVal);
                    }
                }
            }

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
                if (apiResponse != null && apiResponse.Success)
                {
                    return apiResponse.Data!;
                }
                throw new Exception(apiResponse?.Message ?? "Request failed.");
            }
            else
            {
                var errContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(errContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    throw new Exception(apiResponse?.Message ?? "Server error.");
                }
                catch
                {
                    throw new Exception($"HTTP Error {response.StatusCode}: {response.ReasonPhrase}");
                }
            }
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            // Append target API endpoint URI to the MVC response headers so developer can see it in network tab
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && response.RequestMessage != null)
            {
                var apiUri = response.RequestMessage.RequestUri?.ToString();
                var method = response.RequestMessage.Method.Method;
                if (!string.IsNullOrEmpty(apiUri))
                {
                    var headerKey = "X-API-Call";
                    var headerVal = $"{method} {apiUri}";
                    if (!httpContext.Response.Headers.ContainsKey(headerKey))
                    {
                        httpContext.Response.Headers.Add(headerKey, headerVal);
                    }
                    else
                    {
                        httpContext.Response.Headers[headerKey] = string.Concat(httpContext.Response.Headers[headerKey], ", ", headerVal);
                    }
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                var errContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(errContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    throw new Exception(apiResponse?.Message ?? "Server error.");
                }
                catch
                {
                    throw new Exception($"HTTP Error {response.StatusCode}");
                }
            }
        }

        // ==========================================
        // AUTHENTICATION
        // ==========================================

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = await _client.PostAsJsonAsync("api/auth/login", request);
            return await HandleResponse<LoginResponse>(response);
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/auth/change-password", request);
            await HandleResponse(response);
        }

        // ==========================================
        // PATIENTS
        // ==========================================

        public async Task<PagedResult<PatientDto>> GetPatientsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/patients?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<PatientDto>>(response);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/patients/{id}");
            return await HandleResponse<PatientDto>(response);
        }

        public async Task<PatientDto> CreatePatientAsync(PatientDto dto)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/patients", dto);
            return await HandleResponse<PatientDto>(response);
        }

        public async Task<PatientDto> UpdatePatientAsync(int id, PatientDto dto)
        {
            AddAuthHeader();
            var response = await _client.PutAsJsonAsync($"api/patients/{id}", dto);
            return await HandleResponse<PatientDto>(response);
        }

        public async Task DeletePatientAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.DeleteAsync($"api/patients/{id}");
            await HandleResponse(response);
        }

        public async Task<PagedResult<OrderDto>> GetPatientHistoryAsync(int id, int pageNumber, int pageSize)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/patients/{id}/history?pageNumber={pageNumber}&pageSize={pageSize}");
            return await HandleResponse<PagedResult<OrderDto>>(response);
        }

        // ==========================================
        // DOCTORS
        // ==========================================

        public async Task<PagedResult<DoctorDto>> GetDoctorsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/doctors?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<DoctorDto>>(response);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/doctors/{id}");
            return await HandleResponse<DoctorDto>(response);
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorDto dto)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/doctors", dto);
            return await HandleResponse<DoctorDto>(response);
        }

        public async Task<DoctorDto> UpdateDoctorAsync(int id, DoctorDto dto)
        {
            AddAuthHeader();
            var response = await _client.PutAsJsonAsync($"api/doctors/{id}", dto);
            return await HandleResponse<DoctorDto>(response);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.DeleteAsync($"api/doctors/{id}");
            await HandleResponse(response);
        }

        // ==========================================
        // TESTS
        // ==========================================

        public async Task<PagedResult<TestDto>> GetTestsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/tests?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<TestDto>>(response);
        }

        public async Task<TestDto> GetTestByIdAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/tests/{id}");
            return await HandleResponse<TestDto>(response);
        }

        public async Task<TestDto> CreateTestAsync(TestDto dto)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/tests", dto);
            return await HandleResponse<TestDto>(response);
        }

        public async Task<TestDto> UpdateTestAsync(int id, TestDto dto)
        {
            AddAuthHeader();
            var response = await _client.PutAsJsonAsync($"api/tests/{id}", dto);
            return await HandleResponse<TestDto>(response);
        }

        public async Task DeleteTestAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.DeleteAsync($"api/tests/{id}");
            await HandleResponse(response);
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/tests/departments");
            return await HandleResponse<List<DepartmentDto>>(response);
        }

        public async Task<List<SampleTypeDto>> GetSampleTypesAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/tests/sampletypes");
            return await HandleResponse<List<SampleTypeDto>>(response);
        }

        public async Task<List<TestPackageDto>> GetPackagesAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/tests/packages");
            return await HandleResponse<List<TestPackageDto>>(response);
        }

        // ==========================================
        // ORDERS
        // ==========================================

        public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/orders", request);
            return await HandleResponse<OrderDto>(response);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/orders/{id}");
            return await HandleResponse<OrderDto>(response);
        }

        public async Task<PagedResult<OrderDto>> GetOrdersPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/orders?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<OrderDto>>(response);
        }

        public async Task CancelOrderAsync(int id, string remarks)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync($"api/orders/{id}/cancel", remarks);
            await HandleResponse(response);
        }

        // ==========================================
        // SAMPLES
        // ==========================================

        public async Task<SampleDto> GetSampleByIdAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/samples/{id}");
            return await HandleResponse<SampleDto>(response);
        }

        public async Task<PagedResult<SampleDto>> GetSamplesPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/samples?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<SampleDto>>(response);
        }

        public async Task<List<SampleDto>> GetPendingSamplesAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/samples/pending");
            return await HandleResponse<List<SampleDto>>(response);
        }

        public async Task CollectSampleAsync(int sampleId, string barcode, int collectedById)
        {
            AddAuthHeader();
            var payload = new { SampleId = sampleId, Barcode = barcode, CollectedById = collectedById };
            var response = await _client.PostAsJsonAsync("api/samples/collect", payload);
            await HandleResponse(response);
        }

        public async Task ReceiveSampleAsync(int sampleId, int receivedById)
        {
            AddAuthHeader();
            var payload = new { SampleId = sampleId, ReceivedById = receivedById };
            var response = await _client.PostAsJsonAsync("api/samples/receive", payload);
            await HandleResponse(response);
        }

        public async Task RejectSampleAsync(int sampleId, string reason, string? remarks, int rejectedById)
        {
            AddAuthHeader();
            var payload = new { SampleId = sampleId, RejectionReason = reason, Remarks = remarks, RejectedById = rejectedById };
            var response = await _client.PostAsJsonAsync("api/samples/reject", payload);
            await HandleResponse(response);
        }

        // ==========================================
        // CLINICAL RESULTS
        // ==========================================

        public async Task EnterResultsAsync(EnterResultsRequest request)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/results", request);
            await HandleResponse(response);
        }

        public async Task<List<TestResultDto>> GetResultsBySampleIdAsync(int sampleId)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/results/sample/{sampleId}");
            return await HandleResponse<List<TestResultDto>>(response);
        }

        public async Task SubmitResultsForVerificationAsync(int sampleId)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync($"api/results/sample/{sampleId}/submit", new { });
            await HandleResponse(response);
        }

        public async Task<List<TestResultHistoryDto>> GetResultHistoryAsync(int resultId)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/results/{resultId}/history");
            return await HandleResponse<List<TestResultHistoryDto>>(response);
        }

        // ==========================================
        // REPORTS
        // ==========================================

        public async Task<PagedResult<ReportDto>> GetReportsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/reports?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<ReportDto>>(response);
        }

        public async Task<ReportDto> GetReportByIdAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/reports/{id}");
            return await HandleResponse<ReportDto>(response);
        }

        public async Task<ReportDto> GetReportByOrderIdAsync(int orderId)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/reports/order/{orderId}");
            return await HandleResponse<ReportDto>(response);
        }

        public async Task<ReportDto> VerifyReportAsync(int id, string? remarks)
        {
            AddAuthHeader();
            var payload = new { Remarks = remarks };
            var response = await _client.PostAsJsonAsync($"api/reports/{id}/verify", payload);
            return await HandleResponse<ReportDto>(response);
        }

        public async Task<ReportDto> PublishReportAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync($"api/reports/{id}/publish", new { });
            return await HandleResponse<ReportDto>(response);
        }

        public async Task<byte[]> DownloadReportPdfAsync(int id)
        {
            // Direct file download - doesn't use the standard JSON response wrapper
            var response = await _client.GetAsync($"api/reports/{id}/download");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            throw new Exception("Failed to download PDF.");
        }

        public async Task<ReportVerificationResponse> VerifyPublicReportAsync(string reportNumber)
        {
            // No auth header needed (Public URL lookup!)
            var response = await _client.GetAsync($"api/reports/verify/{reportNumber}");
            return await HandleResponse<ReportVerificationResponse>(response);
        }

        // ==========================================
        // BILLING / PAYMENTS
        // ==========================================

        public async Task<PagedResult<InvoiceDto>> GetInvoicesPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/billing/invoices?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<InvoiceDto>>(response);
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(int id)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/billing/invoices/{id}");
            return await HandleResponse<InvoiceDto>(response);
        }

        public async Task UpdateInvoiceDiscountAsync(int invoiceId, decimal discountAmount)
        {
            AddAuthHeader();
            var response = await _client.PutAsJsonAsync($"api/billing/invoices/{invoiceId}/discount", new { DiscountAmount = discountAmount });
            await HandleResponse(response);
        }

        public async Task<PaymentDto> CollectPaymentAsync(CreatePaymentRequest request)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/billing/payments", request);
            return await HandleResponse<PaymentDto>(response);
        }

        public async Task<PagedResult<PaymentDto>> GetPaymentsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/billing/payments?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<PaymentDto>>(response);
        }

        // ==========================================
        // HOME COLLECTIONS
        // ==========================================

        public async Task<HomeCollectionDto> RequestHomeCollectionAsync(RequestHomeCollectionRequest request)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/homecollection", request);
            return await HandleResponse<HomeCollectionDto>(response);
        }

        public async Task AssignHomeCollectionAgentAsync(int hcId, int agentId)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync($"api/homecollection/{hcId}/assign", agentId);
            await HandleResponse(response);
        }

        public async Task UpdateHomeCollectionStatusAsync(int hcId, HomeCollectionStatus status, string? remarks)
        {
            AddAuthHeader();
            var payload = new { Status = status, Remarks = remarks };
            var response = await _client.PostAsJsonAsync($"api/homecollection/{hcId}/status", payload);
            await HandleResponse(response);
        }

        public async Task<PagedResult<HomeCollectionDto>> GetHomeCollectionsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/homecollection?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<HomeCollectionDto>>(response);
        }

        // ==========================================
        // DASHBOARD
        // ==========================================

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/dashboard/summary");
            return await HandleResponse<DashboardSummaryDto>(response);
        }

        public async Task<List<RevenueChartItem>> GetDashboardRevenueChartAsync(int days)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/dashboard/revenue?days={days}");
            return await HandleResponse<List<RevenueChartItem>>(response);
        }

        public async Task<List<TestStatsDto>> GetDashboardTestStatsAsync(int limit)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/dashboard/tests?limit={limit}");
            return await HandleResponse<List<TestStatsDto>>(response);
        }

        public async Task<List<DepartmentStatsDto>> GetDashboardDepartmentStatsAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/dashboard/departments");
            return await HandleResponse<List<DepartmentStatsDto>>(response);
        }

        // ==========================================
        // ADMINISTRATION & AUDITS
        // ==========================================

        public async Task<PagedResult<UserDto>> GetUsersPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/users?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<UserDto>>(response);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            AddAuthHeader();
            var response = await _client.PostAsJsonAsync("api/users", request);
            return await HandleResponse<UserDto>(response);
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/roles");
            return await HandleResponse<List<RoleDto>>(response);
        }

        public async Task<List<PermissionDto>> GetPermissionsAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/permissions");
            return await HandleResponse<List<PermissionDto>>(response);
        }

        public async Task<PagedResult<AuditLogDto>> GetAuditLogsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            AddAuthHeader();
            var response = await _client.GetAsync($"api/auditlogs?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? "")}");
            return await HandleResponse<PagedResult<AuditLogDto>>(response);
        }

        public async Task<byte[]> GetManualPdfAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/reports/download-manual");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            throw new Exception("Failed to download manual PDF from API.");
        }

        public async Task<string> GetTodayLogsAsync()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("api/admin/todaylogs");
            return await HandleResponse<string>(response);
        }
    }
}
