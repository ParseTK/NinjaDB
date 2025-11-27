using Microsoft.AspNetCore.Mvc;
using NinjaDB.Interfaces;
using NinjaDB.Models;
using NinjaDB.API.DTOs;

namespace NinjaDB.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetAll()
    {
        var products = _productService.GetAll();
        var dtos = products.Select(p => new ProductDto
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            UnitPrice = p.UnitPrice
        });
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public ActionResult<ProductDto> GetById(int id)
    {
        var product = _productService.GetById(id);
        if (product == null)
            return NotFound(new { message = $"Product with ID {id} not found" });

        var dto = new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            UnitPrice = product.UnitPrice
        };
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<ProductDto> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            var product = new Products
            {
                ProductName = dto.ProductName,
                UnitPrice = dto.UnitPrice
            };
            _productService.Create(product);

            var resultDto = new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice
            };
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, resultDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found" });

            product.ProductName = dto.ProductName;
            product.UnitPrice = dto.UnitPrice;

            _productService.Update(product);
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
            _productService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}