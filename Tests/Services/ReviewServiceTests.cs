using Chapters;
using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Services;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ReviewSpecs;
using Chapters.Specifications.UserRatingReviewSpecs;
using Chapters.Specifications.UserSpecs;
using Moq;

namespace Tests.Services;

public class ReviewServiceTests
{
    private readonly Mock<IRepository<Review>> _mockReviewRepository = new();
    private readonly Mock<IRepository<UserBook>> _mockUserBookRepository = new();
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Mock<IRepository<UserRatingReview>> _mockUserRatingReviewRepository = new();
    private readonly Mock<IUserActivityService> _mockActivityService = new();
    private readonly ReviewService _reviewService;

    private const string Username = nameof(Username);

    public ReviewServiceTests()
    {
        _reviewService = new ReviewService(
            _mockReviewRepository.Object,
            _mockUserBookRepository.Object,
            _mockUserRepository.Object,
            _mockUserRatingReviewRepository.Object,
            _mockActivityService.Object
        );
    }

    [Fact]
    public async Task GetReviews_ShouldReturnReviewList()
    {
        // Arrange
        var reviewRequest = new GetReviewsRequest { BookId = 1, Username = Username };
        var reviews = new List<Review>
        {
            new()
            {
                Id = 1, 
                Title = "Review 1",
                Text = "Text 1", 
                Author = new User { Username = Username },
                UserRatingReviews = []
            },
            new()
            {
                Id = 2, 
                Title = "Review 2",
                Text = "Text 2", 
                Author = new User { Username = Username },
                UserRatingReviews = []
            }
        };
        _mockReviewRepository
            .Setup(repo => repo.ListAsync(It.IsAny<ReviewWithUserRatingSpec>(), CancellationToken.None))
            .ReturnsAsync(reviews);

        // Act
        var result = await _reviewService.GetReviews(reviewRequest);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task PostReview_ShouldAddReviewAndSaveActivity()
    {
        // Arrange
        var postReviewRequest = new PostReviewRequest { BookId = 1, Title = "New review", Text = "Review text" };
        var user = new User { Id = 1, Username = "username" };
        _mockUserRepository.Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);

        // Act
        await _reviewService.PostReview(Username, postReviewRequest);

        // Assert
        _mockReviewRepository.Verify(
            repo => repo.AddAsync(
                It.Is<Review>(r =>
                    r.BookId == 1 && r.Title == "New review" && r.Text == "Review text" && r.AuthorId == 1),
                CancellationToken.None), Times.Once);
        _mockActivityService.Verify(service => service.SavePostReviewActivity(1, 1), Times.Once);
    }

    [Fact]
    public async Task RateReview_ShouldAddOrUpdateUserRatingReview()
    {
        // Arrange
        const int reviewId = 1;
        const bool isPositive = true;
        var user = new User { Id = 1, Username = Username };
        var userRatingReview = new UserRatingReview { ReviewId = reviewId, UserId = user.Id, UserRating = 0 };
        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockUserRatingReviewRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserRatingReviewSpec>(), CancellationToken.None))
            .ReturnsAsync(userRatingReview);

        // Act
        await _reviewService.RateReview(Username, reviewId, isPositive);

        // Assert
        _mockUserRatingReviewRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.Equal(1, userRatingReview.UserRating);
    }
}