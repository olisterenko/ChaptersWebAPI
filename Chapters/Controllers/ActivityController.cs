using System.Security.Claims;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/activities")]
public class ActivityController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserActivityService _userActivityService;

    public ActivityController(IHttpContextAccessor httpContextAccessor, IUserActivityService userActivityService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userActivityService = userActivityService;
    }

    [HttpGet("{username}")]
    public async Task<List<GetUserActivityResponse>> GetUserActivities(string username)
    {
        return await _userActivityService.GetUserActivities(username);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<List<GetUserActivityResponse>> GetSubscriptionsActivities()
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
        
        return await _userActivityService.GetSubscriptionsActivities(username!.Value);
    }
}