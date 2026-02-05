namespace ProductLocator.Api.Dtos.Auth;

public record LogoutRequest(
    string RefreshToken
);