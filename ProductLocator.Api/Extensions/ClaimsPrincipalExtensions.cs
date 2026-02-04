using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductLocator.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        foreach (var claim in user.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }
        var id = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(id))
        {
            throw new InvalidOperationException("User ID claim is missing.");
        }
        return int.Parse(id!);
    }

    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(username))
        {
            throw new InvalidOperationException("Username claim is missing.");
        }
        return username!;
    }

    public static string GetRole(this ClaimsPrincipal user)
    {
        var role = user.FindFirstValue(ClaimTypes.Role);
        if (string.IsNullOrEmpty(role))
        {
            throw new InvalidOperationException("Role claim is missing.");
        }
        return role!;
    }
}
