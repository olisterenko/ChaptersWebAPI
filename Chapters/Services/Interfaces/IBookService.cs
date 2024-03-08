using Chapters.Requests;
using Chapters.Responses;

namespace Chapters.Services.Interfaces;

public interface IBookService
{
    Task<List<GetBooksResponse>> GetBooks(GetBooksRequest booksRequest);
}