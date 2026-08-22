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
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IOrderService _orderService;

        public PatientsController(IPatientService patientService, IOrderService orderService)
        {
            _patientService = patientService;
            _orderService = orderService;
        }

        [HttpGet]
        [HasPermission("PATIENTS_VIEW")]
        public async Task<ActionResult<ApiResponse<PagedResult<PatientDto>>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _patientService.GetAllPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<PatientDto>>.CreateSuccess(result));
        }

        [HttpGet("{id}")]
        [HasPermission("PATIENTS_VIEW")]
        public async Task<ActionResult<ApiResponse<PatientDto>>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _patientService.GetByIdAsync(id, cancellationToken);
                return Ok(ApiResponse<PatientDto>.CreateSuccess(patient));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost]
        [HasPermission("PATIENTS_CREATE")]
        public async Task<ActionResult<ApiResponse<PatientDto>>> Create([FromBody] PatientDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _patientService.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<PatientDto>.CreateSuccess(result, "Patient registered successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [HasPermission("PATIENTS_EDIT")]
        public async Task<ActionResult<ApiResponse<PatientDto>>> Update(int id, [FromBody] PatientDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _patientService.UpdateAsync(id, dto, cancellationToken);
                return Ok(ApiResponse<PatientDto>.CreateSuccess(result, "Patient profile updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [HasPermission("PATIENTS_DELETE")]
        public async Task<ActionResult<ApiResponse>> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _patientService.DeleteAsync(id, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Patient record deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet("{id}/history")]
        [HasPermission("PATIENTS_VIEW")]
        public async Task<ActionResult<ApiResponse<PagedResult<OrderDto>>>> GetHistory(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            // Fictional wrapper returning patient previous order history list
            var orders = await _orderService.GetOrdersPagedAsync(pageNumber, pageSize, $"PAT-{id:D6}", cancellationToken);
            return Ok(ApiResponse<PagedResult<OrderDto>>.CreateSuccess(orders));
        }
    }
}
