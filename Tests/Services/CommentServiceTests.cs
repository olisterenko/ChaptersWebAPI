using Chapters;
using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Services;
using Chapters.Services.Interfaces;
using Chapters.Specifications.CommentSpecs;
using Chapters.Specifications.UserRatingCommentSpecs;
using Chapters.Specifications.UserSpecs;
using Moq;

namespace Tests.Services;

public class CommentServiceTests
{
    private readonly Mock<IRepository<Comment>> _mockCommentRepository = new();
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Mock<IRepository<UserRatingComment>> _mockUserRatingCommentRepository = new();
    private readonly Mock<IUserActivityService> _mockActivityService = new();
    private readonly CommentService _commentService;

    public CommentServiceTests()
    {
        _commentService = new CommentService(
            _mockCommentRepository.Object,
            _mockUserRepository.Object,
            _mockUserRatingCommentRepository.Object,
            _mockActivityService.Object
        );
    }

    [Fact]
    public async Task GetComments_ShouldReturnCommentList()
    {
        // Arrange
        var commentRequest = new GetCommentRequest { ChapterId = 1, Username = "username" };
        var comments = new List<Comment>
        {
            new()
            {
                Id = 1,
                Text = "Comment1",
                Author = new User { Username = "username" },
                UserRatingComments = []
            },
            new()
            {
                Id = 2,
                Text = "Comment2",
                Author = new User { Username = "username" },
                UserRatingComments = []
            }
        };
        _mockCommentRepository
            .Setup(repo => repo.ListAsync(It.IsAny<CommentWithUserRatingSpec>(), CancellationToken.None))
            .ReturnsAsync(comments);

        // Act
        var result = await _commentService.GetComments(commentRequest);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task PostComment_ShouldAddCommentAndSaveActivity()
    {
        // Arrange
        const string username = nameof(username);
        var postCommentRequest = new PostCommentRequest { ChapterId = 1, Text = "New comment" };
        var user = new User { Id = 1, Username = "username" };
        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);

        // Act
        await _commentService.PostComment(username, postCommentRequest);

        // Assert
        _mockCommentRepository.Verify(
            repo => repo.AddAsync(
                It.Is<Comment>(c => c.ChapterId == 1 && c.Text == "New comment" && c.AuthorId == 1),
                CancellationToken.None),
            Times.Once);
        _mockActivityService.Verify(service => service.SavePostCommentActivity(1, 1), Times.Once);
    }

    [Fact]
    public async Task RateComment_ShouldAddOrUpdateUserRatingComment()
    {
        // Arrange
        const string username = nameof(username);
        const int commentId = 1;
        const bool isPositive = true;

        var user = new User { Id = 1, Username = "username" };
        var userRatingComment = new UserRatingComment { CommentId = commentId, UserId = user.Id, UserRating = 0 };

        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockUserRatingCommentRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserRatingCommentSpec>(), CancellationToken.None))
            .ReturnsAsync(userRatingComment);

        // Act
        await _commentService.RateComment(username, commentId, isPositive);

        // Assert
        _mockUserRatingCommentRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.Equal(1, userRatingComment.UserRating);
    }
}