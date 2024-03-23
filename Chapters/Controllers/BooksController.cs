using System.Security.Claims;
using Chapters.Domain.Enums;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/books")]
public class BooksController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBookService _bookService;

    public BooksController(IHttpContextAccessor httpContextAccessor, IBookService bookService)
    {
        _httpContextAccessor = httpContextAccessor;
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<List<GetBookResponse>> GetBooks([FromQuery] BookStatus? bookStatus = null)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
        return await _bookService.GetBooks(new GetBooksRequest { Username = username?.Value, BookStatus = bookStatus });
    }

    [HttpGet("{bookId:int}")]
    public async Task<GetBookResponse> GetBook(int bookId)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
        return await _bookService.GetBook(new GetBookRequest { Username = username?.Value, BookId = bookId });
    }

    [HttpPost("{bookId:int}/status")]
    public async Task ChangeBookStatus(int bookId, [FromBody] BookStatus bookStatus)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _bookService.ChangeBookStatus(
            new ChangeBookStatusRequest
            {
                Username = username?.Value,
                BookId = bookId,
                NewStatus = bookStatus
            });
    }

    [HttpPost("{bookId:int}/rating")]
    public async Task RateBook(int bookId, [FromBody] int rating)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _bookService.RateBook(
            new RateBookRequest
            {
                Username = username?.Value,
                BookId = bookId,
                NewRating = rating
            });
    }

    // TODO: поиск
}