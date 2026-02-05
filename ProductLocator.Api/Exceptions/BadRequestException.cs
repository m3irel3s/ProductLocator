namespace ProductLocator.Api.Exceptions;

public sealed class BadRequestException : AppException
{
    public BadRequestException(string message) : base(message) { }
    public override int StatusCode => StatusCodes.Status400BadRequest;
}