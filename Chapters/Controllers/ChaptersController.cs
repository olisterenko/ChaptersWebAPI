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

    [HttpGet("{bookId}")]
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
}