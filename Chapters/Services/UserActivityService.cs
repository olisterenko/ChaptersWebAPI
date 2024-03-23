using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.BookSpecs;
using Chapters.Specifications.UserSpecs;
using Chapters.Specifications.UserSubscriberSpecs;

namespace Chapters.Services;

public class UserActivityService : IUserActivityService
{
    private readonly IRepository<UserActivity> _userActivityRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserSubscriber> _userSubscriberRepository;
    private readonly IRepository<Book> _bookRepository;

    public UserActivityService(
        IRepository<UserActivity> userActivityRepository,
        IRepository<User> userRepository,
        IRepository<UserSubscriber> userSubscriberRepository,
        IRepository<Book> bookRepository)
    {
        _userActivityRepository = userActivityRepository;
        _userRepository = userRepository;
        _userSubscriberRepository = userSubscriberRepository;
        _bookRepository = bookRepository;
    }

    public async Task<List<GetUserActivityResponse>> GetUserActivities(string username)
    {
        var user = await _userRepository.FirstAsync(new UserWithActivitySpec(username));

        return user.UserActivities
            .Select(activity => new GetUserActivityResponse(
                Id: activity.Id,
                UserId: user.Id,
                Username: username,
                Text: activity.Text,
                CreatedAt: activity.CreatedAt
            ))
            .ToList();
    }

    public async Task<List<GetUserActivityResponse>> GetSubscriptionsActivities(string username)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(username));

        var subscriptions = await _userSubscriberRepository
            .ListAsync(new SubscriptionsWithUserAndActivitiesSpec(user.Id));

        var subscriptionsActivities = subscriptions
            .SelectMany(subscription => subscription.User.UserActivities)
            .OrderByDescending(us => us.CreatedAt)
            .ToList();

        return subscriptionsActivities
            .Select(activity => new GetUserActivityResponse(
                Id: activity.Id,
                UserId: activity.UserId,
                Username: activity.User.Username,
                Text: activity.Text,
                CreatedAt: activity.CreatedAt
            ))
            .ToList();
    }

    public async Task SaveChangeStatusActivity(int userId, int bookId, BookStatus bookStatus)
    {
        var book = await _bookRepository.FirstAsync(new BookSpec(bookId));

        var text = bookStatus switch
        {
            BookStatus.Finished => $"Заканчивает читать книгу {book.Title}.",
            BookStatus.Reading => $"Читает книгу {book.Title}.",
            BookStatus.WillRead => $"Будет читать книгу {book.Title}.",
            BookStatus.StopReading => $"Перестает читать книгу {book.Title}.",
            _ => $"Не читает книгу {book.Title}."
        };

        var userActivity = new UserActivity
        {
            Text = text,
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _userActivityRepository.AddAsync(userActivity);
    }

    public async Task SaveRateBookActivity(int userId, int bookId, int rating)
    {
        var book = await _bookRepository.FirstAsync(new BookSpec(bookId));

        var userActivity = new UserActivity
        {
            Text = $"ставит {rating} книге {book.Title}.",
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _userActivityRepository.AddAsync(userActivity);
    }

    // TODO: прочтение книги
    // TODO: оценивание главы
    // TODO: прочтение главы
    // TODO: написание коммента
    // TODO: написание рецензии
}