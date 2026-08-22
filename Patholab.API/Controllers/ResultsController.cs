using System;
using System.Collections.Generic;
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
    public class ResultsController : ControllerBase
    {
        private readonly ITestResultService _resultService;

        public ResultsController(ITestResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpPost]
        [HasPermission("RESULTS_ENTER")]
        public async Task<ActionResult<ApiResponse>> EnterResults([FromBody] EnterResultsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

                await _resultService.EnterResultsAsync(request, userId, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Results saved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet("sample/{sampleId}")]
        public async Task<ActionResult<ApiResponse<List<TestResultDto>>>> GetBySampleId(int sampleId, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetResultsBySampleIdAsync(sampleId, cancellationToken);
            return Ok(ApiResponse<List<TestResultDto>>.CreateSuccess(results));
        }

        [HttpPost("sample/{sampleId}/submit")]
        [HasPermission("RESULTS_ENTER")]
        public async Task<ActionResult<ApiResponse>> Submit(int sampleId, CancellationToken cancellationToken)
        {
            try
            {
                await _resultService.SubmitResultsForVerificationAsync(sampleId, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Results submitted for pathology verification successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet("{resultId}/history")]
        public async Task<ActionResult<ApiResponse<List<TestResultHistoryDto>>>> GetHistory(int resultId, CancellationToken cancellationToken)
        {
            var history = await _resultService.GetHistoryAsync(resultId, cancellationToken);
            return Ok(ApiResponse<List<TestResultHistoryDto>>.CreateSuccess(history));
        }
    }
}
