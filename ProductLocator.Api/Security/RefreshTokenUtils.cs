using System.Security.Cryptography;

namespace ProductLocator.Api.Security;

public static class RefreshTokenUtils
{
    public static string GeneratePlainToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public static string Hash(string plainToken)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plainToken));
        return Convert.ToBase64String(bytes);
    }
}
