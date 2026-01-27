using Microsoft.AspNetCore.Diagnostics;

namespace ProductLocator.Api.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            AppException appEx => (appEx.StatusCode, appEx.GetType().Name),
            _ => (StatusCodes.Status500InternalServerError, "InternalServerError")
        };

        var detail = exception is AppException ? exception.Message : "An unexpected error occurred.";

        context.Response.StatusCode = status;

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = status
        }, cancellationToken);

        return true;
    }
}
