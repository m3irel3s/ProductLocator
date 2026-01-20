namespace ProductLocator.Api.Domain;

public class Store
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<StoreMember> StoreMembers { get; set; } = new List<StoreMember>();
    public ICollection<StoreProduct> StoreProducts { get; set; } = new List<StoreProduct>();
}