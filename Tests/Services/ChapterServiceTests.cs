using Chapters;
using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Dto.Requests;
using Chapters.Services;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ChapterSpecs;
using Chapters.Specifications.UserBookSpecs;
using Chapters.Specifications.UserChapterSpecs;
using Chapters.Specifications.UserSpecs;
using Moq;

namespace Tests.Services;

public class ChapterServiceTests
{
    private readonly Mock<IRepository<UserBook>> _mockUserBookRepository = new();
    private readonly Mock<IRepository<Chapter>> _mockChapterRepository = new();
    private readonly Mock<IRepository<Book>> _mockBookRepository = new();
    private readonly Mock<IRepository<UserChapter>> _mockUserChapterRepository = new();
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Mock<IUserActivityService> _mockActivityService = new();
    private readonly ChapterService _chapterService;

    public ChapterServiceTests()
    {
        _chapterService = new ChapterService(
            _mockUserBookRepository.Object,
            _mockChapterRepository.Object,
            _mockBookRepository.Object,
            _mockUserChapterRepository.Object,
            _mockUserRepository.Object,
            _mockActivityService.Object
        );
    }

    [Fact]
    public async Task GetChapters_ShouldReturnChapters()
    {
        // Arrange
        var getChaptersRequest = new GetChaptersRequest { BookId = 1, Username = "username" };
        var chapters = new List<Chapter>
        {
            new() { Id = 1, Number = 1, Title = "Chapter1", UserChapters = [] },
            new() { Id = 2, Number = 2, Title = "Chapter2", UserChapters = [] }
        };

        _mockChapterRepository
            .Setup(repo => repo.ListAsync(It.IsAny<ChaptersWithUserChaptersOrderedSpec>(), CancellationToken.None))
            .ReturnsAsync(chapters);

        // Act
        var result = await _chapterService.GetChapters(getChaptersRequest);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("Chapter1", result[0].Title);
        Assert.Equal(2, result[1].Id);
        Assert.Equal("Chapter2", result[1].Title);
    }

    [Fact]
    public async Task UnmarkChapter_ShouldRemoveUserChapterAndUpdateStatus()
    {
        // Arrange
        var unmarkChapterRequest = new UnmarkChapterRequest { ChapterId = 1, Username = "username" };
        var user = new User { Id = 1, Username = "username" };
        var userChapter = new UserChapter { ChapterId = 1, UserId = 1 };
        var chapter = new Chapter { Id = 1, BookId = 1 };
        var userBook = new UserBook { BookId = 1, UserId = 1, BookStatus = BookStatus.Finished };

        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockUserChapterRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserChapterSpec>(), CancellationToken.None))
            .ReturnsAsync(userChapter);
        _mockChapterRepository
            .Setup(repo => repo.GetByIdAsync(1, CancellationToken.None))
            .ReturnsAsync(chapter);
        _mockUserBookRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserBookSpec>(), CancellationToken.None))
            .ReturnsAsync(userBook);

        // Act
        await _chapterService.UnmarkChapter(unmarkChapterRequest);

        // Assert
        _mockUserChapterRepository.Verify(repo => repo.DeleteAsync(userChapter, CancellationToken.None), Times.Once);
        _mockUserBookRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
        _mockActivityService.Verify(service => service.SaveChangeStatusActivity(1, 1, BookStatus.Reading), Times.Once);
    }

    [Fact]
    public async Task RateChapter_ShouldUpdateUserChapterRating()
    {
        // Arrange
        var rateChapterRequest = new RateChapterRequest { ChapterId = 1, Username = "username", NewRating = 4 };
        var user = new User { Id = 1, Username = "username" };
        var userChapter = new UserChapter { ChapterId = 1, UserId = 1, UserRating = 0 };

        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockUserChapterRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserChapterSpec>(), CancellationToken.None))
            .ReturnsAsync(userChapter);

        // Act
        await _chapterService.RateChapter(rateChapterRequest);

        // Assert
        Assert.Equal(4, userChapter.UserRating);
        _mockUserChapterRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
        _mockActivityService.Verify(service => service.SaveRateChapterActivity(1, 1, 4), Times.Once);
    }
}