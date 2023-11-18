using System;
using System.Collections.Generic;

namespace amazonCloneWebAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal? Price { get; set; }

    public string? Category { get; set; }
}
