namespace ProductLocator.Api.Domain;

public class StoreMember
{
    public Guid StoreId { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Store Store { get; set; } = null!;
    public User User { get; set; } = null!;
}