namespace ProductLocator.Api.Domain;

public class AuditLog
{
    public int Id { get; set; }
    public int? ActorUserId { get; set; }
    public int? StoreId { get; set; }
    public string Action { get; set; } = null!;
    public string? Data { get; set; }
    public DateTime CreatedAt { get; set; }

    public User? ActorUser { get; set; } = null!;
    public Store? Store { get; set; } = null!;
}
