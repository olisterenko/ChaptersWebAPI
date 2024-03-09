using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ReviewSpecs;
using Chapters.Specifications.UserBookSpecs;
using Chapters.Specifications.UserSpecs;

namespace Chapters.Services;

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<UserBook> _userBookRepository;
    private readonly IRepository<User> _userRepository;

    public ReviewService(
        IRepository<Review> reviewRepository,
        IRepository<UserBook> userBookRepository,
        IRepository<User> userRepository)
    {
        _reviewRepository = reviewRepository;
        _userBookRepository = userBookRepository;
        _userRepository = userRepository;
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

    public async Task PostReview(string username, int bookId, PostReviewRequest postReviewRequest)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(username));

        var review = new Review
        {
            BookId = bookId,
            Title = postReviewRequest.Title,
            Text = postReviewRequest.Text,
            AuthorId = user.Id,
            Rating = 0,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _reviewRepository.AddAsync(review);
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