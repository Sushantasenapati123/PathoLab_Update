using System;
using System.Collections.Generic;
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
    public class SamplesController : ControllerBase
    {
        private readonly ISampleService _sampleService;

        public SamplesController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SampleDto>>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var sample = await _sampleService.GetSampleByIdAsync(id, cancellationToken);
                return Ok(ApiResponse<SampleDto>.CreateSuccess(sample));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<SampleDto>>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _sampleService.GetSamplesPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<SampleDto>>.CreateSuccess(result));
        }

        [HttpGet("pending")]
        public async Task<ActionResult<ApiResponse<List<SampleDto>>>> GetPending(CancellationToken cancellationToken)
        {
            var samples = await _sampleService.GetPendingSamplesAsync(cancellationToken);
            return Ok(ApiResponse<List<SampleDto>>.CreateSuccess(samples));
        }

        [HttpPost("collect")]
        [HasPermission("SAMPLES_COLLECT")]
        public async Task<ActionResult<ApiResponse>> Collect([FromBody] CollectSamplePayload payload, CancellationToken cancellationToken)
        {
            try
            {
                await _sampleService.CollectSampleAsync(payload.SampleId, payload.Barcode, payload.CollectedById, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Sample collected and barcode assigned successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("receive")]
        [HasPermission("SAMPLES_RECEIVE")]
        public async Task<ActionResult<ApiResponse>> Receive([FromBody] ReceiveSamplePayload payload, CancellationToken cancellationToken)
        {
            try
            {
                await _sampleService.ReceiveSampleAsync(payload.SampleId, payload.ReceivedById, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Sample received in lab and parameter fields pre-generated."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("reject")]
        [HasPermission("SAMPLES_RECEIVE")]
        public async Task<ActionResult<ApiResponse>> Reject([FromBody] RejectSamplePayload payload, CancellationToken cancellationToken)
        {
            try
            {
                await _sampleService.RejectSampleAsync(payload.SampleId, payload.RejectionReason, payload.Remarks, payload.RejectedById, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Sample rejected successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        public class CollectSamplePayload
        {
            public int SampleId { get; set; }
            public string Barcode { get; set; } = string.Empty;
            public int CollectedById { get; set; }
        }

        public class ReceiveSamplePayload
        {
            public int SampleId { get; set; }
            public int ReceivedById { get; set; }
        }

        public class RejectSamplePayload
        {
            public int SampleId { get; set; }
            public string RejectionReason { get; set; } = string.Empty;
            public string? Remarks { get; set; }
            public int RejectedById { get; set; }
        }
    }
}
