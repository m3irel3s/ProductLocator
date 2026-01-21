namespace ProductLocator.Api.Domain;

public class UserGlobalRole
{
    public int UserId { get; set; }
    public string Role { get; set; } = null!;

    public User User { get; set; } = null!;
}