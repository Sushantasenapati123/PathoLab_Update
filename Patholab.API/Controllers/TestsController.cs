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
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestsController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<TestDto>>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _testService.GetAllPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<TestDto>>.CreateSuccess(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TestDto>>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var test = await _testService.GetByIdAsync(id, cancellationToken);
                return Ok(ApiResponse<TestDto>.CreateSuccess(test));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TestDto>>> Create([FromBody] TestDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _testService.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<TestDto>.CreateSuccess(result, "Test created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TestDto>>> Update(int id, [FromBody] TestDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _testService.UpdateAsync(id, dto, cancellationToken);
                return Ok(ApiResponse<TestDto>.CreateSuccess(result, "Test updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _testService.DeleteAsync(id, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Test deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet("departments")]
        public async Task<ActionResult<ApiResponse<List<DepartmentDto>>>> GetDepartments(CancellationToken cancellationToken)
        {
            var depts = await _testService.GetDepartmentsAsync(cancellationToken);
            return Ok(ApiResponse<List<DepartmentDto>>.CreateSuccess(depts));
        }

        [HttpGet("sampletypes")]
        public async Task<ActionResult<ApiResponse<List<SampleTypeDto>>>> GetSampleTypes(CancellationToken cancellationToken)
        {
            var types = await _testService.GetSampleTypesAsync(cancellationToken);
            return Ok(ApiResponse<List<SampleTypeDto>>.CreateSuccess(types));
        }

        [HttpGet("packages")]
        public async Task<ActionResult<ApiResponse<List<TestPackageDto>>>> GetPackages(CancellationToken cancellationToken)
        {
            var packages = await _testService.GetPackagesAsync(cancellationToken);
            return Ok(ApiResponse<List<TestPackageDto>>.CreateSuccess(packages));
        }
    }
}
