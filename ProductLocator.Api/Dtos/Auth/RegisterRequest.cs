namespace ProductLocator.Api.Dtos.Auth;

public record RegisterRequest(
    [Required]
    [MinLength(3)]
    string Username,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(8)]
    string Password,

    [Required]
    Role Role
)
{
    public string Username { get; init; } = Username.Trim();
    public string Email { get; init; } = Email.Trim().ToLowerInvariant();
}