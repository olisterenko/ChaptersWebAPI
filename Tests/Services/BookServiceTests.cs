using Chapters;
using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Dto.Requests;
using Chapters.Services;
using Chapters.Services.Interfaces;
using Chapters.Specifications.BookSpecs;
using Chapters.Specifications.UserBookSpecs;
using Chapters.Specifications.UserChapterSpecs;
using Chapters.Specifications.UserSpecs;
using Moq;

namespace Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IRepository<Book>> _mockBookRepository = new();
    private readonly Mock<IRepository<UserBook>> _mockUserBookRepository = new();
    private readonly Mock<IRepository<UserChapter>> _mockUserChapterRepository = new();
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Mock<IUserActivityService> _mockActivityService = new();
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _bookService = new BookService(
            _mockBookRepository.Object,
            _mockUserBookRepository.Object,
            _mockUserChapterRepository.Object,
            _mockUserRepository.Object,
            _mockActivityService.Object
        );
    }

    [Fact]
    public async Task GetBooks_WithUsernameAndBookStatus_ShouldReturnBooks()
    {
        // Arrange
        var booksRequest = new GetBooksRequest { Username = "username", BookStatus = BookStatus.Reading };
        var books = new List<Book>
        {
            new() { Id = 1, Title = "The Lord of The Rings", Author = "J.R.R. Tolkien", Cover = "Cover 1" },
            new() { Id = 2, Title = "The Dark Tower", Author = "Steven King", Cover = "Cover 2" }
        };
        _mockBookRepository
            .Setup(repo => repo.ListAsync(It.IsAny<BooksByBookStatusSpec>(), CancellationToken.None))
            .ReturnsAsync(books);

        // Act
        var result = await _bookService.GetBooks(booksRequest);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("The Lord of The Rings", result[0].Title);
        Assert.Equal("The Dark Tower", result[1].Title);
    }

    [Fact]
    public async Task GetBook_WhenValidRequest_ShouldReturnBook()
    {
        // Arrange
        var getBookRequest = new GetBookRequest { BookId = 1, Username = "username" };
        var book = new Book { Id = 1, Title = "The Lord of The Rings", Author = "J.R.R. Tolkien", Cover = "Cover" };
        _mockBookRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<BookWithUserBooksSpec>(), CancellationToken.None))
            .ReturnsAsync(book);

        // Act
        var result = await _bookService.GetBook(getBookRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("The Lord of The Rings", result.Title);
    }

    [Fact]
    public async Task ChangeBookStatus_WhenValidRequest_ShouldChangeStatus()
    {
        // Arrange
        var changeBookStatusRequest = new ChangeBookStatusRequest
        {
            Username = "username",
            BookId = 1,
            NewStatus = BookStatus.Finished
        };
        var user = new User { Id = 1, Username = "username" };
        var userBook = new UserBook { BookId = 1, UserId = 1, BookStatus = BookStatus.Reading };

        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockUserBookRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserBookSpec>(), CancellationToken.None))
            .ReturnsAsync(userBook);

        // Act
        await _bookService.ChangeBookStatus(changeBookStatusRequest);

        // Assert
        _mockUserBookRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
        _mockActivityService.Verify(service => service.SaveChangeStatusActivity(1, 1, BookStatus.Finished), Times.Once);
    }

    [Fact]
    public async Task ChangeBookStatus_WhenChangedToNotStarted_ShouldDeleteUserChaptersAndUserBook()
    {
        // Arrange
        var changeBookStatusRequest = new ChangeBookStatusRequest
        {
            Username = "username",
            BookId = 1,
            NewStatus = BookStatus.NotStarted
        };
        var user = new User { Id = 1, Username = "username" };
        var userBook = new UserBook { BookId = 1, UserId = 1, BookStatus = BookStatus.Reading };

        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockUserBookRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserBookSpec>(), CancellationToken.None))
            .ReturnsAsync(userBook);

        // Act
        await _bookService.ChangeBookStatus(changeBookStatusRequest);

        // Assert
        _mockUserChapterRepository
            .Verify(repo => repo.DeleteRangeAsync(It.IsAny<UserChaptersSpec>(), CancellationToken.None), Times.Once);
        _mockUserBookRepository
            .Verify(repo => repo.DeleteAsync(userBook, CancellationToken.None), Times.Once);
        _mockActivityService
            .Verify(service => service.SaveChangeStatusActivity(1, 1, BookStatus.NotStarted), Times.Once);
    }

    [Fact]
    public async Task RateBook_WhenRatesFirstTime_ShouldUpdateBookRating()
    {
        // Arrange
        var rateBookRequest = new RateBookRequest { Username = "username", BookId = 1, NewRating = 5 };
        var user = new User { Id = 1, Username = "username" };
        var book = new Book { Id = 1, UserBooks = new List<UserBook>() };

        _mockUserRepository.Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockBookRepository.Setup(repo => repo.FirstAsync(It.IsAny<BookWithUserBooksSpec>(), CancellationToken.None))
            .ReturnsAsync(book);

        // Act
        await _bookService.RateBook(rateBookRequest);

        // Assert
        _mockUserBookRepository
            .Verify(
                repo => repo.AddAsync(
                    It.Is<UserBook>(ub => ub.UserRating == 5 && ub.BookStatus == BookStatus.Reading),
                    CancellationToken.None),
                Times.Once);
        _mockActivityService.Verify(service => service.SaveRateBookActivity(1, 1, 5), Times.Once);
        _mockBookRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.Equal(5.0, book.Rating);
    }

    [Fact]
    public async Task SearchBooks_WhenValidRequest_ShouldReturnBooks()
    {
        // Arrange
        var searchBooksRequest = new SearchBooksRequest { Q = "Test", Username = "username" };
        var books = new List<Book>
        {
            new() { Id = 1, Title = "The Lord of The Rings", Author = "J.R.R. Tolkien", Cover = "Cover 1" },
            new() { Id = 2, Title = "The Dark Tower", Author = "Steven King", Cover = "Cover 2" }
        };
        _mockBookRepository
            .Setup(repo => repo.ListAsync(It.IsAny<BooksForSearchSpec>(), CancellationToken.None))
            .ReturnsAsync(books);

        // Act
        var result = await _bookService.SearchBooks(searchBooksRequest);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("The Lord of The Rings", result[0].Title);
        Assert.Equal("The Dark Tower", result[1].Title);
    }
}