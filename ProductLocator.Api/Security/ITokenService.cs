namespace ProductLocator.Api.Security;

public interface ITokenService
{
    string GenerateAccessToken(User user);
}
