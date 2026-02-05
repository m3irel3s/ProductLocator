namespace ProductLocator.Api.Guards;

public sealed class PasswordGuard
{
    public static void EnsureStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new BadRequestException("Password must be at least 8 characters long.");
        if (password.Length < 8)
            throw new BadRequestException("Password must be at least 8 characters long.");
        if (!password.Any(char.IsUpper))
            throw new BadRequestException("Password must contain at least one uppercase letter.");
        if (!password.Any(char.IsLower))
            throw new BadRequestException("Password must contain at least one lowercase letter.");
        if (!password.Any(char.IsDigit))
            throw new BadRequestException("Password must contain at least one digit.");
        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            throw new BadRequestException("Password must contain at least one special character.");
    }
}