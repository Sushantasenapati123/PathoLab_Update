using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.API.Authorization;
using Patholab.Application.Interfaces;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpGet("invoices")]
        [HasPermission("BILLING_VIEW")]
        public async Task<ActionResult<ApiResponse<PagedResult<InvoiceDto>>>> GetInvoices(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _billingService.GetInvoicesPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<InvoiceDto>>.CreateSuccess(result));
        }

        [HttpGet("invoices/{id}")]
        [HasPermission("BILLING_VIEW")]
        public async Task<ActionResult<ApiResponse<InvoiceDto>>> GetInvoiceById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await _billingService.GetInvoiceByIdAsync(id, cancellationToken);
                return Ok(ApiResponse<InvoiceDto>.CreateSuccess(invoice));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("payments")]
        [HasPermission("BILLING_PAY")]
        public async Task<ActionResult<ApiResponse<PaymentDto>>> CollectPayment([FromBody] CreatePaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var receivedBy = User.FindFirst(ClaimTypes.Name)?.Value ?? "System";
                var result = await _billingService.CollectPaymentAsync(request, receivedBy, cancellationToken);
                return Ok(ApiResponse<PaymentDto>.CreateSuccess(result, "Payment recorded successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet("payments")]
        [HasPermission("BILLING_VIEW")]
        public async Task<ActionResult<ApiResponse<PagedResult<PaymentDto>>>> GetPayments(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _billingService.GetPaymentsPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<PaymentDto>>.CreateSuccess(result));
        }
    }
}
