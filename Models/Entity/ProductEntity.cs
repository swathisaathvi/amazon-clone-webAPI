namespace amazonCloneWebAPI.Models.Entity;
public class ProductEntity
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal? ProductPrice { get; set; }

    public string? Category { get; set; }
    public string? Status { get; set; }
}
