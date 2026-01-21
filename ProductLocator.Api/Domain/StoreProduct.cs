namespace ProductLocator.Api.Domain;

public class StoreProduct
{
    public int StoreId { get; set; }
    public int ProductId { get; set; }

    public decimal Price { get; set; }
    public string? Aisle { get; set; }
    public string? Shelf { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Store Store { get; set; } = null!;
    public Product Product { get; set; } = null!;
}