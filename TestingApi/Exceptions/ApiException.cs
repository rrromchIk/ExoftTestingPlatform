namespace TestingAPI.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; set; }

    public ApiException()
    {
    }

    public ApiException(string? message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}