using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Requests;
using Chapters.Responses;
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

    public async Task<List<GetBooksResponse>> GetBooks(GetBooksRequest booksRequest)
    {
        List<Book> books;
        if (booksRequest is { Username: not null, BookStatus: not null })
        {
            books = await _bookRepository.ListAsync(new BooksByBookStatusSpec(booksRequest.BookStatus.Value));
        } 
        else if (booksRequest.Username is not null)
        {
            books = await _bookRepository.ListAsync(new BooksForRatingSpec());
        }
        else
        {
            books = await _bookRepository.ListAsync(new BooksOrderedByRatingSpec());
        }

        return books.Select(
            book => new GetBooksResponse(
                Id: book.Id,
                Title: book.Title,
                Author: book.Author,
                Rating: book.Rating,
                Cover: book.Cover,
                BookStatus: book.UserBooks.FirstOrDefault(u => u.User.Username == booksRequest.Username)?.BookStatus ?? BookStatus.NotStarted,
                UserRating: book.UserBooks.FirstOrDefault(u => u.User.Username == booksRequest.Username)?.UserRating ?? 0
            )
        ).ToList();
    }
}