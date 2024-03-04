﻿using Chapters.Entities;
using Chapters.Filters;
using Chapters.Requests;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[UserExceptionFilter]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task Register(CreateUserRequest request)
    {
        await _userService.CreateUser(request);
    }

    [HttpGet("unprotected")]
    public IActionResult GetUnprotected()
    {
        // This endpoint is not protected by authentication
        return Ok("This endpoint is accessible without authentication.");
    }
}