namespace ProductLocator.Api.Dtos.Auth;

public record UserSummary(
    int Id,
    string Username,
    string Email,
    Role Role
);