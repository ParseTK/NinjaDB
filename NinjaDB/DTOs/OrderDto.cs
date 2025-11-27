namespace NinjaDB.API.DTOs;

public class OrderDto
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string OrderDate { get; set; } = string.Empty; // Use string for DateOnly
}

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateOrderDto
{
    public int Quantity { get; set; }
}