using System.Security.Claims;
using Chapters.Domain.Enums;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Filters;
using Chapters.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[ValidationExceptionFilter]
[Route("/api/books")]
public class BooksController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBookService _bookService;
    private readonly IValidator<int> _validator;

    public BooksController(
        IHttpContextAccessor httpContextAccessor,
        IBookService bookService,
        IValidator<int> validator)
    {
        _httpContextAccessor = httpContextAccessor;
        _bookService = bookService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<List<GetBookResponse>> GetBooks([FromQuery] string? name = null, [FromQuery] BookStatus? bookStatus = null)
    {
        var username = name ?? _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        return await _bookService.GetBooks(new GetBooksRequest { Username = username, BookStatus = bookStatus });
    }

    [HttpGet("{bookId:int}")]
    public async Task<GetBookResponse> GetBook(int bookId)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
        return await _bookService.GetBook(new GetBookRequest { Username = username?.Value, BookId = bookId });
    }

    [Authorize]
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

    [Authorize]
    [HttpPost("{bookId:int}/rating")]
    public async Task RateBook(int bookId, [FromBody] int rating)
    {
        await _validator.ValidateAndThrowAsync(rating);

        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _bookService.RateBook(
            new RateBookRequest
            {
                Username = username?.Value,
                BookId = bookId,
                NewRating = rating
            });
    }

    [HttpPost]
    public async Task<List<GetBookResponse>> SearchBooks([FromQuery] string q)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        return await _bookService.SearchBooks(new SearchBooksRequest { Username = username?.Value, Q = q });
    }
}