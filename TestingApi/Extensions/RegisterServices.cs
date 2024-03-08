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
        serviceCollection.AddScoped<IQuestionsPoolService, QuestionsPoolService>();
        serviceCollection.AddScoped<IQuestionService, QuestionService>();
        serviceCollection.AddScoped<IAnswerService, AnswerService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IUserAnswerService, UserAnswerService>();
        serviceCollection.AddScoped<IUserTestService, UserTestService>();
        serviceCollection.AddScoped<IFileService, FileService>();
        serviceCollection.AddScoped<ITestTmplService, TestTmplService>();
        serviceCollection.AddScoped<IQuestionsPoolTmplService, QuestionsPoolTmplService>();
    }
}