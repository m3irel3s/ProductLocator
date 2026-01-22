namespace ProductLocator.Api.Domain;

public class StoreProduct
{
    public int StoreId { get; set; }
    public int ProductId { get; set; }

    public decimal Price { get; set; }
    public int? AisleId { get; set; }
    public int? ShelfNumber { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Store Store { get; set; } = null!;
    public StoreAisle? Aisle { get; set; }
    public Product Product { get; set; } = null!;
}
