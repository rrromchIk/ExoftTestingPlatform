using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Constants;
using TestingApi.Dto.StatisticDto;
using TestingApi.Middlewares;
using TestingApi.Services.Abstractions;
using TestingApi.Services.Implementations;

namespace TestingApi.Extensions;

public static class RegisterServices
{
    public static void RegisterCustomServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<GlobalExceptionHandlingMiddleware>();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddScoped<ITestService, TestService>();
        serviceCollection.AddScoped<IQuestionsPoolService, QuestionsPoolService>();
        serviceCollection.AddScoped<IQuestionService, QuestionService>();
        serviceCollection.AddScoped<IAnswerService, AnswerService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IUserAnswerService, UserAnswerService>();
        serviceCollection.AddScoped<IUserTestService, UserTestService>();
        serviceCollection.AddScoped<IFileService, FileService>();
        serviceCollection.AddScoped<ITestTmplService, TestTmplService>();
        serviceCollection.AddScoped<IQuestionsPoolTmplService, QuestionsPoolTmplService>();
        serviceCollection.AddScoped<IQuestionTmplService, QuestionTmplService>();
        serviceCollection.AddScoped<IAnswerTmplService, AnswerTmplService>();
        serviceCollection.AddScoped<IUserQuestionService, UserQuestionService>();
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddScoped<ICurrentUserService, CurrentUserService>();
        serviceCollection.Configure<SecurityHttpClientConstants>(configuration.GetSection("SecurityHttpClient"));
        serviceCollection.Configure<SuperAdminSeedData>(configuration.GetSection("SuperAdminSeedData"));
        serviceCollection.AddScoped<IUserStatisticService, UserStatisticService>();
        serviceCollection.AddScoped<ITestStatisticService, TestStatisticService>();
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
                    ValidIssuer = configuration["Auth:Issuer"],
                
                    ValidateAudience = true,
                    ValidAudience = configuration["Auth:Audience"],
            
                    ValidateLifetime = true,
                
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Auth:SecretKey"])),
                };
            });
    }

    public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient(configuration["SecurityHttpClient:ClientName"], client =>
        {
            client.BaseAddress = new Uri(configuration["SecurityHttpClient:BaseAddress"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/problem+json"));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
        });
    }
}