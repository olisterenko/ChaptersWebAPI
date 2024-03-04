using Chapters.Requests;
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
}