using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.BookSpecs;
using Chapters.Specifications.UserBookSpecs;
using Chapters.Specifications.UserChapterSpecs;
using Chapters.Specifications.UserSpecs;

namespace Chapters.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<UserBook> _userBookRepository;
    private readonly IRepository<UserChapter> _userChapterRepository;
    private readonly IRepository<User> _userRepository;
    private readonly UserActivityService _activityService;

    public BookService(
        IRepository<Book> bookRepository,
        IRepository<UserBook> userBookRepository,
        IRepository<UserChapter> userChapterRepository,
        IRepository<User> userRepository, UserActivityService activityService)
    {
        _bookRepository = bookRepository;
        _userBookRepository = userBookRepository;
        _userChapterRepository = userChapterRepository;
        _userRepository = userRepository;
        _activityService = activityService;
    }

    public async Task<List<GetBookResponse>> GetBooks(GetBooksRequest booksRequest)
    {
        List<Book> books;
        if (booksRequest is { Username: not null, BookStatus: not null })
        {
            books = await _bookRepository.ListAsync(new BooksByBookStatusSpec(booksRequest.BookStatus.Value));
        }
        else
        {
            books = await _bookRepository.ListAsync(new BooksForTopSpec());
        }

        return books
            .Select(book => GetBookResponse(booksRequest.Username, book))
            .ToList();
    }

    public async Task<GetBookResponse> GetBook(GetBookRequest getBookRequest)
    {
        var book = await _bookRepository.FirstAsync(new BookWithUserBooksSpec(getBookRequest.BookId));

        return GetBookResponse(getBookRequest.Username, book);
    }

    public async Task ChangeBookStatus(ChangeBookStatusRequest changeBookStatusRequest)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(changeBookStatusRequest.Username!));

        var userBook = await _userBookRepository
            .FirstOrDefaultAsync(new UserBookSpec(user.Id, changeBookStatusRequest.BookId));

        if (userBook is null && changeBookStatusRequest.NewStatus != BookStatus.NotStarted)
        {
            userBook = new UserBook
            {
                BookId = changeBookStatusRequest.BookId,
                UserId = user.Id,
                BookStatus = changeBookStatusRequest.NewStatus
            };

            await _userBookRepository.AddAsync(userBook);
            await _activityService.SaveChangeStatusActivity(
                user.Id,
                changeBookStatusRequest.BookId,
                changeBookStatusRequest.NewStatus);

            return;
        }

        if (userBook is null)
        {
            return;
        }

        if (changeBookStatusRequest.NewStatus == BookStatus.NotStarted)
        {
            await _userChapterRepository
                .DeleteRangeAsync(new UserChaptersSpec(user.Id, changeBookStatusRequest.BookId));

            await _userBookRepository.DeleteAsync(userBook);

            await _activityService.SaveChangeStatusActivity(
                user.Id,
                changeBookStatusRequest.BookId,
                changeBookStatusRequest.NewStatus);

            return;
        }

        userBook.BookStatus = changeBookStatusRequest.NewStatus;
        await _userBookRepository.SaveChangesAsync();

        await _activityService.SaveChangeStatusActivity(
            user.Id,
            changeBookStatusRequest.BookId,
            changeBookStatusRequest.NewStatus);
    }

    public async Task RateBook(RateBookRequest rateBookRequest)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(rateBookRequest.Username!));

        var userBook = await _userBookRepository
            .FirstOrDefaultAsync(new UserBookSpec(user.Id, rateBookRequest.BookId));

        if (userBook is null && rateBookRequest.NewRating != 0)
        {
            userBook = new UserBook
            {
                BookId = rateBookRequest.BookId,
                UserId = user.Id,
                UserRating = rateBookRequest.NewRating,
                BookStatus = BookStatus.Reading
            };

            await _userBookRepository.AddAsync(userBook);
            return;
        }

        if (userBook is null)
        {
            return;
        }

        userBook.UserRating = rateBookRequest.NewRating;

        await _userBookRepository.SaveChangesAsync();
    }

    public async Task<List<GetBookResponse>> SearchBooks(SearchBooksRequest searchBooksRequest)
    {
        var books = await _bookRepository.ListAsync(new BooksForSearchSpec(searchBooksRequest.Q));

        return books
            .Select(book => GetBookResponse(searchBooksRequest.Username, book))
            .ToList();
    }

    private static GetBookResponse GetBookResponse(string? username, Book book)
    {
        var bookStatus = BookStatus.NotStarted;
        var userRating = 0;
        if (username is not null)
        {
            bookStatus = book.UserBooks
                .FirstOrDefault(u => u.User.Username == username)?
                .BookStatus ?? BookStatus.NotStarted;

            userRating = book.UserBooks
                .FirstOrDefault(u => u.User.Username == username)?
                .UserRating ?? 0;
        }

        return new GetBookResponse(
            Id: book.Id,
            Title: book.Title,
            Author: book.Author,
            Cover: book.Cover,
            Rating: book.UserBooks.Count != 0
                ? book.UserBooks.Average(userBook => userBook.UserRating)
                : 0.0,
            BookStatus: bookStatus,
            UserRating: userRating
        );
    }
}