using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.BookSpecs;

namespace Chapters.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _bookRepository;

    public BookService(IRepository<Book> bookRepository)
    {
        _bookRepository = bookRepository;
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