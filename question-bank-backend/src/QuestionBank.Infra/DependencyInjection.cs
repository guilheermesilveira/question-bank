using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts.Repositories;
using QuestionBank.Infra.Repositories;

namespace QuestionBank.Infra;

public static class DependencyInjection
{
    public static void ConfigInfra(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigDataBase(services, configuration);
        ConfigRepositoryDependency(services);
    }

    private static void ConfigDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            options.UseMySql(connectionString, serverVersion);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }

    private static void ConfigRepositoryDependency(this IServiceCollection services)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserAnswerRepository, UserAnswerRepository>()
            .AddScoped<ITopicRepository, TopicRepository>()
            .AddScoped<IQuestionRepository, QuestionRepository>()
            .AddScoped<IAlternativeRepository, AlternativeRepository>();
    }
}