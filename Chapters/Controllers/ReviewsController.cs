using System.Security.Claims;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/reviews")]
public class ReviewsController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IReviewService _reviewService;

    public ReviewsController(IHttpContextAccessor httpContextAccessor, IReviewService reviewService)
    {
        _httpContextAccessor = httpContextAccessor;
        _reviewService = reviewService;
    }

    [HttpGet("{bookId}")]
    public async Task<List<GetReviewResponse>> GetReviews(int bookId)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        return await _reviewService.GetReviews(
            new GetReviewsRequest
            {
                Username = username?.Value,
                BookId = bookId
            }
        );
    }
    
    [HttpPost("{bookId:int}")]
    public async Task PostReview(int bookId, PostReviewRequest postReviewRequest)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _reviewService.PostReview(username!.Value, bookId, postReviewRequest);
    }
}