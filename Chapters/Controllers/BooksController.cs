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

    // TODO: менять статус

    // TODO: оценивать книги

    // TODO: поиск
}