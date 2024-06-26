﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/test")]
public class AuthenticationController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public IActionResult Ping()
    {
        return Ok("Authenticated user accessed this endpoint successfully.");
    }
}