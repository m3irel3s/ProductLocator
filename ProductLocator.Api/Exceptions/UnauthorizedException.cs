namespace ProductLocator.Api.Exceptions;

public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message) { }
    public override int StatusCode => StatusCodes.Status401Unauthorized;
}
