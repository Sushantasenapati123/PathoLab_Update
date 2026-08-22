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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [HasPermission("ORDERS_CREATE")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> Create([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _orderService.CreateOrderAsync(request, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<OrderDto>.CreateSuccess(result, "Order registered and billed successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<OrderDto>>>> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken cancellationToken = default)
        {
            var result = await _orderService.GetOrdersPagedAsync(pageNumber, pageSize, search, cancellationToken);
            return Ok(ApiResponse<PagedResult<OrderDto>>.CreateSuccess(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id, cancellationToken);
                return Ok(ApiResponse<OrderDto>.CreateSuccess(order));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.CreateError(ex.Message));
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<ApiResponse>> Cancel(int id, [FromBody] string remarks, CancellationToken cancellationToken)
        {
            try
            {
                await _orderService.CancelOrderAsync(id, remarks, cancellationToken);
                return Ok(ApiResponse.CreateSuccess("Order cancelled successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.CreateError(ex.Message));
            }
        }
    }
}
