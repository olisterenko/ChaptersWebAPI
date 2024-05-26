using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Chapters.Domain.Entities;
using Chapters.Exceptions;
using Chapters.Services.Interfaces;
using Chapters.Specifications.UserSpecs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/test")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Login()
    {
        string? username;
        string? password;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split([':'], 2);
            username = credentials[0];
            password = credentials[1];
        }
        catch
        {
            return BadRequest("Invalid header");
        }
        
        var token = await _authService.LoginAsync(username, password);
        return Ok(token);
    }
}