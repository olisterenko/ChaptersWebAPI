using Chapters.Entities;
using Chapters.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Authorize] // Require authorization for this endpoint
    public IActionResult Get()
    {
        // Only authenticated users can access this endpoint
        return Ok("Authenticated user accessed this endpoint successfully.");
    }

    [HttpGet("unprotected")]
    public async Task<IActionResult> GetUnprotected()
    {
        // This endpoint is not protected by authentication
        return Ok("This endpoint is accessible without authentication.");
    }
}