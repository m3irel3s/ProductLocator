namespace ProductLocator.Api.Dtos.Auth;

public record RegisterResponse(
    int UserId,
    string Username,
    string Email
);