using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using TestingAPI.Exceptions;

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
            _logger.LogError("{msg}, {stc}", e.Message, e.StackTrace);
            var problemDetails = e switch
            {
                ApiException  apiException => new ProblemDetails()
                {
                    Status = apiException.StatusCode,
                    Title = "Api exception occured",
                    Detail = apiException.Message
                },
                _ => new ProblemDetails()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Server error",
                    Detail = e.Message
                }
            };
                
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = ContentType.ApplicationJson.ToString();

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}