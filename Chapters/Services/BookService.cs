using Chapters.Entities;
using Chapters.Enums;
using Chapters.Requests;
using Chapters.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications;

namespace Chapters.Services;

public class BookService : IBookService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Book> _bookRepository;

    public BookService(IRepository<User> userRepository, IRepository<Book> bookRepository)
    {
        _userRepository = userRepository;
        _bookRepository = bookRepository;
    }

    public async Task<GetBookResponse> GetBook(string username, int bookId)
    {
        var book = await _bookRepository.FirstAsync(new BookByIdSpec(bookId));
        var user = await _userRepository.FirstAsync(new UserWithBooksSpec(username));
        var userBook = user.UserBooks.First(x => x.BookId == book.Id);
        return new GetBookResponse(
            Id: book.Id,
            Title: book.Title,
            Rating: book.Rating,
            Author: book.Author,
            YearWritten: book.YearWritten,
            Cover: null,
            BookStatus: userBook.BookStatus,
            UserRating: userBook.UserRating,
            Chapters: book.Chapters.Select(ch => new GetCompactChapterResponse(
                Id: ch.Id,
                Title: ch.Title,
                UserRating: 0,
                Rating: 0,
                Status: false,
                ReadDate: DateTime.Today)
            ).ToList()
        );
    }

    public async Task<List<GetBooksResponse>> GetBooks(GetBooksRequest booksRequest)
    {
        List<Book> books;
        if (booksRequest is { Username: not null, BookStatus: not null })
        {
            books = await _bookRepository.ListAsync(new BooksByBookStatusSpec(booksRequest.BookStatus.Value));
        } 
        else if (booksRequest.Username is not null)
        {
            books = await _bookRepository.ListAsync(new BooksWithUserDetailsOrderedByRatingSpec());
        }
        else
        {
            books = await _bookRepository.ListAsync(new BooksOrderedByRatingSpec());
        }

        return books.Select(
            x => new GetBooksResponse(
                Id: x.Id,
                Title: x.Title,
                Author: x.Author,
                Rating: x.Rating,
                Cover: x.Cover,
                BookStatus: x.UserBooks.FirstOrDefault(u => u.User.Username == booksRequest.Username)?.BookStatus ?? BookStatus.NotStarted,
                UserRating: x.UserBooks.FirstOrDefault(u => u.User.Username == booksRequest.Username)?.UserRating ?? 0
            )
        ).ToList();
    }
}