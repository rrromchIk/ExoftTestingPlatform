using TestingApi.Middlewares;
using TestingApi.Services.Abstractions;
using TestingApi.Services.Implementations;

namespace TestingApi.Extensions;

public static class RegisterServices
{
    public static void RegisterCustomServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GlobalExceptionHandlingMiddleware>();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddScoped<ITestService, TestService>();
    }
}