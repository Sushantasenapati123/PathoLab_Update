using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.API.Authorization;
using Patholab.Application.Interfaces;
using Patholab.Domain.Enums;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HomeCollectionController : ControllerBase
    {
        private readonly IHomeCollectionService _hcService;

        public HomeCollectionController(IHomeCollectionService hcService)
        {
            _hcService = hcService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<HomeCollectionDto>>> RequestCollection([FromBody] RequestHomeCollectionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _hcService.RequestCollectionAsync(request, cancellationToken);
                return Ok(ApiResponse<HomeCollectionDto>.CreateSuccess(result, "Home collection request created."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<HomeCollectionDto>>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _hcService.GetPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<HomeCollectionDto>>.CreateSuccess(result));
        }

        [HttpPost("{id}/assign")]
        public async Task<ActionResult<ApiResponse>> AssignAgent(int id, [FromBody] int agentId, CancellationToken cancellationToken)
        {
            try
            {
                await _hcService.AssignAgentAsync(id, agentId, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Collection agent assigned successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("{id}/status")]
        public async Task<ActionResult<ApiResponse>> UpdateStatus(int id, [FromBody] UpdateStatusPayload payload, CancellationToken cancellationToken)
        {
            try
            {
                await _hcService.UpdateStatusAsync(id, payload.Status, payload.Remarks, cancellationToken);
                return Ok(ApiResponse.CreateSuccess($"Home collection status updated to {payload.Status}."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        public class UpdateStatusPayload
        {
            public HomeCollectionStatus Status { get; set; }
            public string? Remarks { get; set; }
        }
    }
}
