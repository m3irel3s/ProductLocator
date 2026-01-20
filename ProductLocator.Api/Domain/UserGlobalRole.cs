namespace ProductLocator.Api.Domain;

public class UserGlobalRole
{
    public Guid UserId { get; set; }
    public string Role { get; set; } = null!;

    public User User { get; set; } = null!;
}