using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestionBank.Application.Configurations;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.Notifications;
using QuestionBank.Application.Services;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra;
using ScottBrady91.AspNetCore.Identity;

namespace QuestionBank.Application;

public static class DependencyInjection
{
    public static void ConfigApplication(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigJwtAndStorage(services, configuration);
        ConfigServiceDependency(services);
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.ConfigInfra(configuration);
    }

    private static void ConfigJwtAndStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    }

    private static void ConfigServiceDependency(this IServiceCollection services)
    {
        services
            .AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>()
            .AddScoped<INotificator, Notificator>();

        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ITopicService, TopicService>()
            .AddScoped<IQuestionService, QuestionService>()
            .AddScoped<IAlternativeService, AlternativeService>();
    }
}