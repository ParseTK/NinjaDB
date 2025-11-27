namespace NinjaDB.API.DTOs;

public class ProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}

public class CreateProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}

public class UpdateProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}