using Microsoft.AspNetCore.Mvc;
using NinjaDB.Interfaces;
using NinjaDB.Models;
using NinjaDB.API.DTOs;

namespace NinjaDB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<OrderDto>> GetAll()
    {
        var orders = _orderService.GetAll();
        var dtos = orders.Select(o => new OrderDto
        {
            OrderId = o.OrderId,
            CustomerId = o.CustomerId,
            ProductId = o.ProductId,
            Quantity = o.Quantity,
            UnitPrice = o.UnitPrice,
            OrderDate = o.OrderDate.ToString("yyyy-MM-dd")
        });
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public ActionResult<OrderDto> GetById(int id)
    {
        var order = _orderService.GetById(id);
        if (order == null)
            return NotFound(new { message = $"Order with ID {id} not found" });

        var dto = new OrderDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            ProductId = order.ProductId,
            Quantity = order.Quantity,
            UnitPrice = order.UnitPrice,
            OrderDate = order.OrderDate.ToString("yyyy-MM-dd")
        };
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<OrderDto> Create([FromBody] CreateOrderDto dto)
    {
        try
        {
            var order = new Orders
            {
                CustomerId = dto.CustomerId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
            _orderService.Create(order);

            var resultDto = new OrderDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                UnitPrice = order.UnitPrice,
                OrderDate = order.OrderDate.ToString("yyyy-MM-dd")
            };
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, resultDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateOrderDto dto)
    {
        try
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound(new { message = $"Order with ID {id} not found" });

            order.Quantity = dto.Quantity;

            _orderService.Update(order);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _orderService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}