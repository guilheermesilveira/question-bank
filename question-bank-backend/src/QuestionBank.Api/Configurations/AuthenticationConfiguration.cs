using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using QuestionBank.Application.Configurations;

namespace QuestionBank.Api.Configurations;

public static class AuthenticationConfiguration
{
    public static void AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettingsSection = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(appSettingsSection);

        var appSettings = appSettingsSection.Get<JwtSettings>();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddAuthorization();

        services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(appSettings.KeyPath));

        services.AddJwksManager().UseJwtValidation();

        services.AddMemoryCache();
    }

    public static void UseAuthConfig(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}