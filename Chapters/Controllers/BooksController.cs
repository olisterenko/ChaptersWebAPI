using System.Security.Claims;
using Chapters.Entities;
using Chapters.Requests;
using Chapters.Responses;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BooksController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBookService _bookService;

    public BooksController(IHttpContextAccessor httpContextAccessor, IBookService bookService)
    {
        _httpContextAccessor = httpContextAccessor;
        _bookService = bookService;
    }

    [HttpPost, Authorize]
    public async Task<GetBookResponse> GetBook(GetBookRequest request)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
        if (username is null)
        {
            throw new InvalidOperationException();
        }

        return await _bookService.GetBook(username.Value, request);
    }
}