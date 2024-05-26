using Chapters;
using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Services;
using Chapters.Specifications.BookSpecs;
using Chapters.Specifications.ChapterSpecs;
using Chapters.Specifications.UserSpecs;
using Moq;

namespace Tests.Services;

public class UserActivityServiceTests
{
    private readonly Mock<IRepository<UserActivity>> _mockUserActivityRepository = new();
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Mock<IRepository<UserSubscriber>> _mockUserSubscriberRepository = new();
    private readonly Mock<IRepository<Book>> _mockBookRepository = new();
    private readonly Mock<IRepository<Chapter>> _mockChapterRepository = new();
    private readonly UserActivityService _userActivityService;

    public UserActivityServiceTests()
    {
        _userActivityService = new UserActivityService(
            _mockUserActivityRepository.Object,
            _mockUserRepository.Object,
            _mockUserSubscriberRepository.Object,
            _mockBookRepository.Object,
            _mockChapterRepository.Object
        );
    }

    [Fact]
    public async Task GetUserActivities_ShouldReturnActivityList()
    {
        // Arrange
        const string username = nameof(username);

        var user = new User
        {
            Id = 1,
            Username = username,
            UserActivities =
            [
                new UserActivity { Id = 1, Text = "Activity1", CreatedAt = DateTimeOffset.UtcNow },
                new UserActivity { Id = 2, Text = "Activity2", CreatedAt = DateTimeOffset.UtcNow }
            ]
        };

        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserWithActivitySpec>(), CancellationToken.None))
            .ReturnsAsync(user);

        // Act
        var result = await _userActivityService.GetUserActivities(username);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Activity1", result[0].Text);
        Assert.Equal("Activity2", result[1].Text);
    }

    [Fact]
    public async Task SaveChangeStatusActivity_ShouldSaveActivity()
    {
        // Arrange
        const int userId = 1;
        const int bookId = 1;
        const BookStatus bookStatus = BookStatus.Reading;
        var book = new Book { Id = bookId, Title = "The Lord of The Rings" };

        _mockBookRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<BookSpec>(), CancellationToken.None))
            .ReturnsAsync(book);

        // Act
        await _userActivityService.SaveChangeStatusActivity(userId, bookId, bookStatus);

        // Assert
        _mockUserActivityRepository.Verify(
            repo => repo.AddAsync(
                It.Is<UserActivity>(ua => ua.Text == "Читает книгу The Lord of The Rings." && ua.UserId == userId),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task SaveRateBookActivity_ShouldSaveActivity()
    {
        // Arrange
        const int userId = 1;
        const int bookId = 1;
        const int rating = 5;
        var book = new Book { Id = bookId, Title = "The Lord of The Rings" };

        _mockBookRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<BookSpec>(), CancellationToken.None))
            .ReturnsAsync(book);

        // Act
        await _userActivityService.SaveRateBookActivity(userId, bookId, rating);

        // Assert
        _mockUserActivityRepository.Verify(
            repo => repo.AddAsync(
                It.Is<UserActivity>(ua =>
                    ua.Text == $"Ставит {rating} книге The Lord of The Rings." && ua.UserId == userId),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task SaveRateChapterActivity_ShouldSaveActivity()
    {
        // Arrange
        const int userId = 1;
        const int chapterId = 1;
        const int rating = 5;
        var chapter = new Chapter { Id = chapterId, Title = "Test Chapter", BookId = 1 };
        var book = new Book { Id = 1, Title = "The Lord of The Rings" };

        _mockChapterRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<ChapterSpec>(), CancellationToken.None))
            .ReturnsAsync(chapter);
        _mockBookRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<BookSpec>(), CancellationToken.None))
            .ReturnsAsync(book);

        // Act
        await _userActivityService.SaveRateChapterActivity(userId, chapterId, rating);

        // Assert
        _mockUserActivityRepository.Verify(
            repo => repo.AddAsync(
                It.Is<UserActivity>(ua =>
                    ua.Text == $"Ставит {rating} главе Test Chapter книги The Lord of The Rings." && ua.UserId == userId),
                CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task SaveReadChapterActivity_ShouldSaveActivity()
    {
        // Arrange
        const int userId = 1;
        const int chapterId = 1;
        var chapter = new Chapter { Id = chapterId, Title = "Test Chapter", BookId = 1 };
        var book = new Book { Id = 1, Title = "The Lord of The Rings" };

        _mockChapterRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<ChapterSpec>(), CancellationToken.None))
            .ReturnsAsync(chapter);
        _mockBookRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<BookSpec>(), CancellationToken.None))
            .ReturnsAsync(book);

        // Act
        await _userActivityService.SaveReadChapterActivity(userId, chapterId);

        // Assert
        _mockUserActivityRepository.Verify(
            repo => repo.AddAsync(
                It.Is<UserActivity>(ua =>
                    ua.Text == "Прочитывает главу Test Chapter книги The Lord of The Rings." && ua.UserId == userId),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task SavePostCommentActivity_ShouldSaveActivity()
    {
        // Arrange
        const int userId = 1;
        const int chapterId = 1;

        var chapter = new Chapter { Id = chapterId, Title = "Test Chapter", BookId = 1 };
        var book = new Book { Id = 1, Title = "The Lord of The Rings" };

        _mockChapterRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<ChapterSpec>(), CancellationToken.None))
            .ReturnsAsync(chapter);
        _mockBookRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<BookSpec>(), CancellationToken.None))
            .ReturnsAsync(book);

        // Act
        await _userActivityService.SavePostCommentActivity(userId, chapterId);

        // Assert
        _mockUserActivityRepository.Verify(
            repo => repo.AddAsync(
                It.Is<UserActivity>(ua =>
                    ua.Text == "Пишет комментарий к главе Test Chapter книги The Lord of The Rings." && ua.UserId == userId),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task SavePostReviewActivity_ShouldSaveActivity()
    {
        // Arrange
        const int userId = 1;
        const int bookId = 1;
        var book = new Book { Id = bookId, Title = "The Lord of The Rings" };

        _mockBookRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<BookSpec>(), CancellationToken.None))
            .ReturnsAsync(book);

        // Act
        await _userActivityService.SavePostReviewActivity(userId, bookId);

        // Assert
        _mockUserActivityRepository.Verify(
            repo => repo.AddAsync(
                It.Is<UserActivity>(ua =>
                    ua.Text == "Пишет рецензию на книгу The Lord of The Rings." && ua.UserId == userId),
                CancellationToken.None),
            Times.Once);
    }
}