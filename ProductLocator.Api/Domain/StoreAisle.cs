namespace ProductLocator.Api.Domain;

public class StoreAisle
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string Name { get; set; } = null!;
    public int MaxShelf { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Store Store { get; set; } = null!;
    public ICollection<StoreProduct> StoreProducts { get; set; } = new List<StoreProduct>();
}