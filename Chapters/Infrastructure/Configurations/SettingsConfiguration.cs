using Chapters.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SettingsConfiguration
{
    public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PasswordHasherSettings>(configuration.GetSection("PasswordHasherSettings"));
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        return services;
    }
}