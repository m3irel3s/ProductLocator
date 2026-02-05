namespace ProductLocator.Api.Exceptions;

public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message) { }
    public override int StatusCode => StatusCodes.Status403Forbidden;
}