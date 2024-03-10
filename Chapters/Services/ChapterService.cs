using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ChapterSpecs;
using Chapters.Specifications.UserChapterSpecs;
using Chapters.Specifications.UserSpecs;

namespace Chapters.Services;

public class ChapterService : IChapterService
{
    private readonly IRepository<Chapter> _chapterRepository;
    private readonly IRepository<UserChapter> _userChapterRepository;
    private readonly IRepository<User> _userRepository;

    public ChapterService(
        IRepository<Chapter> chapterRepository,
        IRepository<UserChapter> userChapterRepository,
        IRepository<User> userRepository)
    {
        _chapterRepository = chapterRepository;
        _userChapterRepository = userChapterRepository;
        _userRepository = userRepository;
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
        var user = await _userRepository.FirstAsync(new UserSpec(markChapterRequest.Username));

        var userChapter = new UserChapter
        {
            ChapterId = markChapterRequest.ChapterId,
            UserId = user.Id,
            IsRead = true,
            ReadTime = DateTimeOffset.UtcNow
        };

        await _userChapterRepository.AddAsync(userChapter);
        
        // TODO: проверять количество прочитанных и закрывать книгу
    }

    public async Task UnmarkChapter(UnmarkChapterRequest unmarkChapterRequest)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(unmarkChapterRequest.Username));
        var chapter = await _userChapterRepository
            .FirstAsync(new UserChapterSpec(user.Id, unmarkChapterRequest.ChapterId));
        
        await _userChapterRepository.DeleteAsync(chapter);
    }

    private GetChapterResponse GetChapterResponse(GetChaptersRequest chaptersRequest, Chapter chapter)
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