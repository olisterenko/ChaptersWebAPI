using Chapters.Dto.Requests;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IBookService
{
    Task<List<GetBookResponse>> GetBooks(GetBooksRequest booksRequest);
    Task<GetBookResponse> GetBook(GetBookRequest getBookRequest);
    Task ChangeBookStatus(ChangeBookStatusRequest changeBookStatusRequest);
    Task RateBook(RateBookRequest rateBookRequest);
    Task<List<GetBookResponse>> SearchBooks(SearchBooksRequest searchBooksRequest);
}