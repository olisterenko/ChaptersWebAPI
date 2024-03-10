using Chapters.Services;
using Chapters.Services.Interfaces;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserActivityService, UserActivityService>();
        services.AddScoped<ISubscriberService, SubscriberService>();
        return services;
    }
}