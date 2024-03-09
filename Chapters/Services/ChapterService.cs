using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ChapterSpecs;

namespace Chapters.Services;

public class ChapterService : IChapterService
{
    private readonly IRepository<Chapter> _chapterRepository;

    public ChapterService(IRepository<Chapter> chapterRepository)
    {
        _chapterRepository = chapterRepository;
    }

    public async Task<List<GetChapterResponse>> GetChapters(GetChaptersRequest chaptersRequest)
    {
        var chapters = await _chapterRepository
            .ListAsync(new ChaptersWithUserChaptersOrderedSpec(chaptersRequest.BookId));


        return chapters.Select(chapter => GetChapterResponse(chaptersRequest, chapter))
            .ToList();
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