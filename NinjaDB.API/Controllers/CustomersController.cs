using Microsoft.AspNetCore.Mvc;
using NinjaDB.Interfaces;
using NinjaDB.Models;
using NinjaDB.API.DTOs;

namespace NinjaDB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetAll()
    {
        var customers = _customerService.GetAll();
        var dtos = customers.Select(c => new CustomerDto
        {
            CustomerId = c.CustomerId,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email
        });
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public ActionResult<CustomerDto> GetById(int id)
    {
        var customer = _customerService.GetById(id);
        if (customer == null)
            return NotFound(new { message = $"Customer with ID {id} not found" });

        var dto = new CustomerDto
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email
        };
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<CustomerDto> Create([FromBody] CreateCustomerDto dto)
    {
        try
        {
            var customer = new Customers
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };
            _customerService.Create(customer);

            var resultDto = new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };
            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, resultDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        try
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
                return NotFound(new { message = $"Customer with ID {id} not found" });

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;

            _customerService.Update(customer);
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
            _customerService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}