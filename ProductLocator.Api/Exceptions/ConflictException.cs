namespace ProductLocator.Api.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message) { }
    public override int StatusCode => StatusCodes.Status409Conflict;
}