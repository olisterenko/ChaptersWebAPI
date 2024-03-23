using Chapters.Dto.Requests;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IChapterService
{
    Task<List<GetChapterResponse>> GetChapters(GetChaptersRequest chaptersRequest);
    Task MarkChapter(MarkChapterRequest markChapterRequest, int rating = 0);
    Task UnmarkChapter(UnmarkChapterRequest unmarkChapterRequest);
    Task RateChapter(RateChapterRequest rateChapterRequest);
}