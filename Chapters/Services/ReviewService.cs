using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ReviewSpecs;
using Chapters.Specifications.UserBookSpecs;

namespace Chapters.Services;

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<UserBook> _userBookRepository;

    public ReviewService(IRepository<Review> reviewRepository, IRepository<UserBook> userBookRepository)
    {
        _reviewRepository = reviewRepository;
        _userBookRepository = userBookRepository;
    }

    public async Task<List<GetReviewResponse>> GetReviews(GetReviewsRequest reviewsRequest)
    {
        var reviews = await _reviewRepository
            .ListAsync(new ReviewWithUserRatingSpec(reviewsRequest.BookId));

        var responses = new List<GetReviewResponse>();
        foreach(var review in reviews)
        {
            var response = await GetReviewResponse(reviewsRequest, review);
            responses.Add(response);
        }

        return responses;
    }

    private async Task<GetReviewResponse> GetReviewResponse(GetReviewsRequest reviewsRequest, Review review)
    {
        var userRating = 0;
        if (reviewsRequest.Username is not null)
        {
            userRating = review.UserRatingReviews
                .FirstOrDefault(u => u.User.Username == reviewsRequest.Username)?
                .UserRating ?? 0;
        }

        var userBook = await _userBookRepository.FirstOrDefaultAsync(new UserBookSpec(review.AuthorId, review.BookId));

        return new GetReviewResponse(
            Id: review.Id,
            AuthorId: review.AuthorId,
            AuthorUsername: review.Author.Username,
            AuthorBookRating: userBook?.UserRating ?? 0,
            Title: review.Title,
            Text: review.Text,
            Rating: review.UserRatingReviews.Sum(ur => ur.UserRating),
            UserRating: userRating,
            CreatedAt: review.CreatedAt
        );
    }
}