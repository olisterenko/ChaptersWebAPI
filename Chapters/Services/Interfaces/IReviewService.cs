using Chapters.Dto.Requests;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IReviewService
{
    Task<List<GetReviewResponse>> GetReviews(GetReviewsRequest reviewsRequest);
    Task PostReview(string username, PostReviewRequest postReviewRequest);
    Task<List<GetUserReviewResponse>> GetUserReviews(GetUserReviewRequest getUserReviewRequest);
}