using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patholab.Application.Interfaces;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> GetSummary(CancellationToken cancellationToken)
        {
            var summary = await _dashboardService.GetDashboardSummaryAsync(cancellationToken);
            return Ok(ApiResponse<DashboardSummaryDto>.CreateSuccess(summary));
        }

        [HttpGet("revenue")]
        public async Task<ActionResult<ApiResponse<List<RevenueChartItem>>>> GetRevenueChart([FromQuery] int days = 7, CancellationToken cancellationToken = default)
        {
            var data = await _dashboardService.GetRevenueChartDataAsync(days, cancellationToken);
            return Ok(ApiResponse<List<RevenueChartItem>>.CreateSuccess(data));
        }

        [HttpGet("tests")]
        public async Task<ActionResult<ApiResponse<List<TestStatsDto>>>> GetTestStats([FromQuery] int limit = 5, CancellationToken cancellationToken = default)
        {
            var stats = await _dashboardService.GetTestStatsAsync(limit, cancellationToken);
            return Ok(ApiResponse<List<TestStatsDto>>.CreateSuccess(stats));
        }

        [HttpGet("departments")]
        public async Task<ActionResult<ApiResponse<List<DepartmentStatsDto>>>> GetDepartmentStats(CancellationToken cancellationToken)
        {
            var stats = await _dashboardService.GetDepartmentStatsAsync(cancellationToken);
            return Ok(ApiResponse<List<DepartmentStatsDto>>.CreateSuccess(stats));
        }
    }
}
