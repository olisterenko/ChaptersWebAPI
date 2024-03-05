using Chapters.Entities;
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

    public async Task<GetBookResponse> GetBook(string username, GetBookRequest bookRequest)
    {
        var book = await _bookRepository.FirstAsync(new BookByIdSpec(bookRequest.BookId));
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
}