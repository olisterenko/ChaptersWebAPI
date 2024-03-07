using Chapters.Options;
using Chapters.Requests;
using Chapters.Services;
using Chapters.Services.Interfaces;
using Chapters.Validators;
using FluentValidation;

namespace Chapters.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IBookService, BookService>();
        return services;
    }
    public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PasswordHasherOptions>(configuration.GetSection("PasswordHasherSettings"));
        return services;
    }
}