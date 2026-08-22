using System;
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
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<DoctorDto>>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _doctorService.GetAllPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<DoctorDto>>.CreateSuccess(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DoctorDto>>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var doctor = await _doctorService.GetByIdAsync(id, cancellationToken);
                return Ok(ApiResponse<DoctorDto>.CreateSuccess(doctor));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DoctorDto>>> Create([FromBody] DoctorDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _doctorService.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<DoctorDto>.CreateSuccess(result, "Doctor profile created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DoctorDto>>> Update(int id, [FromBody] DoctorDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _doctorService.UpdateAsync(id, dto, cancellationToken);
                return Ok(ApiResponse<DoctorDto>.CreateSuccess(result, "Doctor profile updated successfully."));
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
                await _doctorService.DeleteAsync(id, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Doctor record deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }
    }
}
