using Chapters.Dto.Requests;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IBookService
{
    Task<List<GetBooksResponse>> GetBooks(GetBooksRequest booksRequest);
}