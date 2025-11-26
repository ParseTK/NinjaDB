using System;
using System.Collections.Generic;

namespace NinjaDB.Models;

public partial class Products
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
