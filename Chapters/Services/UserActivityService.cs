using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.BookSpecs;
using Chapters.Specifications.ChapterSpecs;
using Chapters.Specifications.UserSpecs;
using Chapters.Specifications.UserSubscriberSpecs;

namespace Chapters.Services;

public class UserActivityService : IUserActivityService
{
    private readonly IRepository<UserActivity> _userActivityRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserSubscriber> _userSubscriberRepository;
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Chapter> _chapterRepository;

    public UserActivityService(
        IRepository<UserActivity> userActivityRepository,
        IRepository<User> userRepository,
        IRepository<UserSubscriber> userSubscriberRepository,
        IRepository<Book> bookRepository,
        IRepository<Chapter> chapterRepository)
    {
        _userActivityRepository = userActivityRepository;
        _userRepository = userRepository;
        _userSubscriberRepository = userSubscriberRepository;
        _bookRepository = bookRepository;
        _chapterRepository = chapterRepository;
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
            Text = $"Ставит {rating} книге {book.Title}.",
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _userActivityRepository.AddAsync(userActivity);
    }

    public async Task SaveRateChapterActivity(int userId, int chapterId, int rating)
    {
        var chapter = await _chapterRepository.FirstAsync(new ChapterSpec(chapterId));
        var book = await _bookRepository.FirstAsync(new BookSpec(chapter.BookId));

        var userActivity = new UserActivity
        {
            Text = $"Ставит {rating} главе {chapter.Title} книги {book.Title}.",
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _userActivityRepository.AddAsync(userActivity);
    }

    public async Task SaveReadChapterActivity(int userId, int chapterId)
    {
        var chapter = await _chapterRepository.FirstAsync(new ChapterSpec(chapterId));
        var book = await _bookRepository.FirstAsync(new BookSpec(chapter.BookId));

        var userActivity = new UserActivity
        {
            Text = $"Прочитывает главу {chapter.Title} книги {book.Title}.",
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _userActivityRepository.AddAsync(userActivity);
    }

    public async Task SavePostCommentActivity(int userId, int chapterId)
    {
        var chapter = await _chapterRepository.FirstAsync(new ChapterSpec(chapterId));
        var book = await _bookRepository.FirstAsync(new BookSpec(chapter.BookId));

        var userActivity = new UserActivity
        {
            Text = $"Пишет комментарий к главе {chapter.Title} книги {book.Title}.",
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _userActivityRepository.AddAsync(userActivity);
    }

    public async Task SavePostReviewActivity(int userId, int bookId)
    {
        var book = await _bookRepository.FirstAsync(new BookSpec(bookId));

        var userActivity = new UserActivity
        {
            Text = $"Пишет рецензию на книгу {book.Title}.",
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _userActivityRepository.AddAsync(userActivity);
    }
}