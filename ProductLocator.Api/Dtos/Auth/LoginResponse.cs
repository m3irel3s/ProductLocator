namespace ProductLocator.Api.Dtos.Auth;

public record LoginResponse(
    UserSummary User,
    string AccessToken,
    string RefreshToken
);