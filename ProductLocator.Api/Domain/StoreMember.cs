namespace ProductLocator.Api.Domain;

public class StoreMember
{
    public int StoreId { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Store Store { get; set; } = null!;
    public User User { get; set; } = null!;
}