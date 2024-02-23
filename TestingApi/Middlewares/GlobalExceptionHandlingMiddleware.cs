using Azure.Core;

namespace TestingApi.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            _logger.LogError("{dt}. {error}", DateTime.Now.ToString(), e.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = ContentType.ApplicationJson.ToString();

            await context.Response.WriteAsJsonAsync(
                new
                {
                    Message = e.Message
                }
            );
        }
    }
}