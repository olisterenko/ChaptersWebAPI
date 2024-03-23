using Chapters.Dto.Requests;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IBookService
{
    Task<List<GetBookResponse>> GetBooks(GetBooksRequest booksRequest);
    Task<GetBookResponse> GetBook(GetBookRequest getBookRequest);
}