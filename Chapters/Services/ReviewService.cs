using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ReviewSpecs;

namespace Chapters.Services;

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _reviewRepository;

    public ReviewService(IRepository<Review> reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<GetReviewResponse>> GetChapters(GetReviewsRequest reviewsRequest)
    {
        var reviews = await _reviewRepository
            .ListAsync(new ReviewWithUserRatingSpec(reviewsRequest.BookId));

        return reviews
            .Select(review => GetReviewResponse(reviewsRequest, review))
            .ToList();
    }

    private GetReviewResponse GetReviewResponse(GetReviewsRequest reviewsRequest, Review review)
    {
        var userRating = 0;
        if (reviewsRequest.Username is not null)
        {
            userRating = review.UserRatingReviews
                .FirstOrDefault(u => u.User.Username == reviewsRequest.Username)?
                .UserRating ?? 0;
        }

        return new GetReviewResponse(
            Id: review.Id,
            AuthorId: review.AuthorId,
            AuthorUsername: review.Author.Username,
            Title: review.Title,
            Text: review.Text,
            Rating: review.UserRatingReviews.Sum(ur => ur.UserRating),
            UserRating: userRating,
            CreatedAt: review.CreatedAt
        );
    }
}