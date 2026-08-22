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
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IManualPdfGenerator _manualPdfGenerator;

        public ReportsController(IReportService reportService, IManualPdfGenerator manualPdfGenerator)
        {
            _reportService = reportService;
            _manualPdfGenerator = manualPdfGenerator;
        }

        [HttpGet("download-manual")]
        [AllowAnonymous]
        public IActionResult DownloadManual()
        {
            var pdfBytes = _manualPdfGenerator.GenerateManualPdf();
            return File(pdfBytes, "application/pdf", "Patholab_User_Manual.pdf");
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ReportDto>>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _reportService.GetReportsPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<ReportDto>>.CreateSuccess(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ReportDto>>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var report = await _reportService.GetReportByIdAsync(id, cancellationToken);
                return Ok(ApiResponse<ReportDto>.CreateSuccess(report));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("{id}/verify")]
        [HasPermission("REPORTS_VERIFY")]
        public async Task<ActionResult<ApiResponse<ReportDto>>> Verify(int id, [FromBody] VerifyReportPayload payload, CancellationToken cancellationToken)
        {
            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

                var report = await _reportService.VerifyReportAsync(id, userId, payload.Remarks, cancellationToken);
                return Ok(ApiResponse<ReportDto>.CreateSuccess(report, "Report verified by pathologist."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("{id}/publish")]
        [HasPermission("REPORTS_PUBLISH")]
        public async Task<ActionResult<ApiResponse<ReportDto>>> Publish(int id, CancellationToken cancellationToken)
        {
            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

                var report = await _reportService.PublishReportAsync(id, userId, cancellationToken);
                return Ok(ApiResponse<ReportDto>.CreateSuccess(report, "Report PDF generated and published successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet("{id}/download")]
        [AllowAnonymous] // Allow direct PDF downloading for demo printing
        public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
        {
            try
            {
                var bytes = await _reportService.DownloadReportPdfAsync(id, cancellationToken);
                return File(bytes, "application/pdf", $"Report_{id}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet("verify/{reportNumber}")]
        [AllowAnonymous] // Public QR Code verification endpoint!
        public async Task<ActionResult<ApiResponse<ReportVerificationResponse>>> VerifyPublic(string reportNumber, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reportService.VerifyPublicReportAsync(reportNumber, cancellationToken);
                return Ok(ApiResponse<ReportVerificationResponse>.CreateSuccess(result, "Report status verified."));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.CreateError(ex.Message));
            }
        }

        public class VerifyReportPayload
        {
            public string? Remarks { get; set; }
        }
    }
}
