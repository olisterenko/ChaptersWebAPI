using System.Security.Claims;
using Chapters.Domain.Enums;
using Chapters.Requests;
using Chapters.Responses;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<List<GetBooksResponse>> GetBooksWithUserDetails([FromQuery] BookStatus? bookStatus = null)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
        return await _bookService.GetBooks(new GetBooksRequest{Username = username?.Value, BookStatus = bookStatus});
    }
}