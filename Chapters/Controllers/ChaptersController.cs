using System.Security.Claims;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/chapters")]
public class ChaptersController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IChapterService _chapterService;

    public ChaptersController(IHttpContextAccessor httpContextAccessor, IChapterService chapterService)
    {
        _httpContextAccessor = httpContextAccessor;
        _chapterService = chapterService;
    }

    [HttpGet("{bookId:int}")]
    public async Task<List<GetChapterResponse>> GetChapters(int bookId)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        return await _chapterService.GetChapters(
            new GetChaptersRequest
            {
                Username = username?.Value,
                BookId = bookId
            }
        );
    }
    
    [HttpPost("{chapterId:int}")]
    public async Task MarkChapter(int chapterId)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _chapterService.MarkChapter(
            new MarkChapterRequest
            {
                Username = username!.Value,
                ChapterId = chapterId
            }
        );
    }
    
    [HttpDelete("{chapterId:int}")]
    public async Task UnmarkChapter(int chapterId)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _chapterService.UnmarkChapter(
            new UnmarkChapterRequest
            {
                Username = username!.Value,
                ChapterId = chapterId
            }
        );
    }
    
    [HttpPost("{chapterId:int}/rating")]
    public async Task RateChapter(int chapterId, [FromBody] int rating)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _chapterService.RateChapter(
            new RateChapterRequest
            {
                Username = username?.Value,
                ChapterId = chapterId,
                NewRating = rating
            });
    }
}