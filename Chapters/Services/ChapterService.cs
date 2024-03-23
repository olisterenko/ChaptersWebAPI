using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ChapterSpecs;
using Chapters.Specifications.UserBookSpecs;
using Chapters.Specifications.UserChapterSpecs;
using Chapters.Specifications.UserSpecs;

namespace Chapters.Services;

public class ChapterService : IChapterService
{
    private readonly IRepository<UserBook> _userBookRepository;
    private readonly IRepository<Chapter> _chapterRepository;
    private readonly IRepository<UserChapter> _userChapterRepository;
    private readonly IRepository<User> _userRepository;
    private readonly UserActivityService _activityService;

    public ChapterService(
        IRepository<UserBook> userBookRepository,
        IRepository<Chapter> chapterRepository,
        IRepository<UserChapter> userChapterRepository,
        IRepository<User> userRepository,
        UserActivityService activityService)
    {
        _userBookRepository = userBookRepository;
        _chapterRepository = chapterRepository;
        _userChapterRepository = userChapterRepository;
        _userRepository = userRepository;
        _activityService = activityService;
    }

    public async Task<List<GetChapterResponse>> GetChapters(GetChaptersRequest chaptersRequest)
    {
        var chapters = await _chapterRepository
            .ListAsync(new ChaptersWithUserChaptersOrderedSpec(chaptersRequest.BookId));


        return chapters.Select(chapter => GetChapterResponse(chaptersRequest, chapter))
            .ToList();
    }

    public async Task MarkChapter(MarkChapterRequest markChapterRequest)
    {
        var user = await _userRepository.FirstAsync(new UserWithChaptersSpec(markChapterRequest.Username));

        var userChapter = new UserChapter
        {
            ChapterId = markChapterRequest.ChapterId,
            UserId = user.Id,
            IsRead = true,
            ReadTime = DateTimeOffset.UtcNow
        };

        await _userChapterRepository.AddAsync(userChapter);

        var chapter = await _chapterRepository.GetByIdAsync(markChapterRequest.ChapterId);
        var userBook = await _userBookRepository
            .FirstOrDefaultAsync(new UserBookWithBookSpec(user.Id, chapter.BookId));

        if (userBook is null)
        {
            userBook = new UserBook
            {
                BookId = chapter.BookId,
                UserId = user.Id,
                BookStatus = BookStatus.Reading
            };

            await _userBookRepository.AddAsync(userBook);
        }

        var readChapters = user.UserChapters
            .Where(uc => uc.Chapter.BookId == chapter.BookId)
            .ToList();

        if (readChapters.Count == userBook.Book.Chapters.Count)
        {
            userBook.BookStatus = BookStatus.Finished;

            await _userBookRepository.SaveChangesAsync();
        }

        await _activityService.SaveChangeStatusActivity(user.Id, userBook.BookId, userBook.BookStatus);
    }

    public async Task UnmarkChapter(UnmarkChapterRequest unmarkChapterRequest)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(unmarkChapterRequest.Username));
        var userChapter = await _userChapterRepository
            .FirstAsync(new UserChapterSpec(user.Id, unmarkChapterRequest.ChapterId));

        var chapter = await _chapterRepository.GetByIdAsync(userChapter.ChapterId);
        var userBook = await _userBookRepository
            .FirstAsync(new UserBookSpec(user.Id, chapter.BookId));

        if (userBook.BookStatus == BookStatus.Finished)
        {
            userBook.BookStatus = BookStatus.Reading;
            await _userBookRepository.SaveChangesAsync();

            await _activityService.SaveChangeStatusActivity(user.Id, userBook.BookId, userBook.BookStatus);
        }

        await _userChapterRepository.DeleteAsync(userChapter);
    }

    public async Task RateChapter(RateChapterRequest rateChapterRequest)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(rateChapterRequest.Username!));

        var userChapter = await _userChapterRepository
            .FirstOrDefaultAsync(new UserChapterSpec(user.Id, rateChapterRequest.ChapterId));

        if (userChapter is null && rateChapterRequest.NewRating != 0)
        {
            userChapter = new UserChapter
            {
                ChapterId = rateChapterRequest.ChapterId,
                UserId = user.Id,
                UserRating = rateChapterRequest.NewRating,
                IsRead = true,
                ReadTime = DateTimeOffset.UtcNow
            };

            await _userChapterRepository.AddAsync(userChapter);
            // TODO: await _activityService.SaveReadChapterActivity(user.Id, userChapter.ChapterId);
            await _activityService.SaveRateChapterActivity(user.Id, userChapter.ChapterId, userChapter.UserRating);

            return;
        }

        if (userChapter is null)
        {
            return;
        }

        userChapter.UserRating = rateChapterRequest.NewRating;
        await _userChapterRepository.SaveChangesAsync();

        await _activityService.SaveRateChapterActivity(user.Id, userChapter.ChapterId, userChapter.UserRating);
    }

    private static GetChapterResponse GetChapterResponse(GetChaptersRequest chaptersRequest, Chapter chapter)
    {
        var isRead = false;
        var userRating = 0;
        if (chaptersRequest.Username is not null)
        {
            isRead = chapter.UserChapters
                .FirstOrDefault(u => u.User.Username == chaptersRequest.Username)?
                .IsRead ?? false;

            userRating = chapter.UserChapters
                .FirstOrDefault(u => u.User.Username == chaptersRequest.Username)?
                .UserRating ?? 0;
        }

        var ratingChapters = chapter.UserChapters
            .Where(ch => ch.UserRating != 0)
            .ToList();

        return new GetChapterResponse(
            Id: chapter.Id,
            Number: chapter.Number,
            Title: chapter.Title,
            Rating: ratingChapters.Count != 0
                ? ratingChapters.Average(userChapter => userChapter.UserRating)
                : 0.0,
            IsRead: isRead,
            UserRating: userRating
        );
    }
}