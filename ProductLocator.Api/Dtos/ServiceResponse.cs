namespace ProductLocator.Api.Dtos;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public int StatusCode { get; set; } = 200;
    public string? Message { get; set; }
}

public static class ServiceResponse
{
    public static ServiceResponse<T> Ok<T>(T data, string? message = null)
        => new()
        {
            Data = data,
            Success = true,
            StatusCode = 200,
            Message = message
        };

    public static ServiceResponse<T> Created<T>(T data, string? message = null)
        => new()
        {
            Data = data,
            Success = true,
            StatusCode = 201,
            Message = message
        };


    public static ServiceResponse<T> Fail<T>(string message, int statusCode)
        => new()
        {
            Data = default,
            Success = false,
            StatusCode = statusCode,
            Message = message
        };
}
