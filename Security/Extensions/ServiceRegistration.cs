using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        services.Configure<SuperAdminSeedData>(configuration.GetSection("SuperAdminSeedData"));
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.Configure<MailSendingSettings>(configuration.GetSection("MailSendingSettings"));
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.Configure<MailTemplatesConstants>(configuration.GetSection("MailTemplatesSettings"));
    }
    
    
    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["AuthSettings:Issuer"],
                
                    ValidateAudience = true,
                    ValidAudience = configuration["AuthSettings:Audience"],
            
                    ValidateLifetime = true,
                
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AuthSettings:SecretKey"])),
                };
            });
    }
}