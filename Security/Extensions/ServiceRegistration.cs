using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Data;
using Security.Middlewares;
using Security.Models;
using Security.Service.Abstractions;
using Security.Service.Implementations;
using Security.Settings;

namespace Security.Extensions;

public static class ServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<GlobalExceptionHandlingMiddleware>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
    }
}