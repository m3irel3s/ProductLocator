namespace ProductLocator.Api.Domain;

public class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string TokenHash { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public int? ReplacedByTokenId { get; set; }
    public RefreshToken? ReplacedByToken { get; set; }

    public bool IsActive =>
        RevokedAt == null &&
        DateTime.UtcNow < ExpiresAt;
}