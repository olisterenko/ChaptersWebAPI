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

    [HttpGet("{bookId:int}")]
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
    
    [HttpPost]
    public async Task PostReview(PostReviewRequest postReviewRequest)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _reviewService.PostReview(username!.Value, postReviewRequest);
    }
    
    [HttpGet("user")]
    public async Task<List<GetUserReviewResponse>> GetUserReviews([FromBody] string author)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        return await _reviewService.GetUserReviews(
            new GetUserReviewRequest
            {
                Author = author,
                Username = username?.Value
            });
    }

    [HttpPost("{reviewId:int}")]
    public async Task RateReview(int reviewId, [FromBody] bool isPositive)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _reviewService.RateReview(username!.Value, reviewId, isPositive);
    }
}