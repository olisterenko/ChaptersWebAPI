using Chapters.Requests;
using Chapters.Responses;

namespace Chapters.Services.Interfaces;

public interface IBookService
{
    Task<GetBookResponse> GetBook(string username, GetBookRequest bookRequest);
}