using System.Security.Claims;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/subscribers")]
public class SubscribersController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISubscriberService _subscriberService;

    public SubscribersController(IHttpContextAccessor httpContextAccessor, ISubscriberService subscriberService)
    {
        _httpContextAccessor = httpContextAccessor;
        _subscriberService = subscriberService;
    }
    
    [HttpGet("{username}")]
    public async Task<List<GetSubscriptionsResponse>> GetSubscriptions(string username)
    {
        return await _subscriberService.GetSubscriptions(username);
    }
    
    [Authorize]
    [HttpPost]
    public async Task Subscribe([FromBody] int userId)
    {
        var subscriberUsername = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        
        await _subscriberService.Subscribe(subscriberUsername!, userId);
    }
    
    [Authorize]
    [HttpDelete]
    public async Task Unsubscribe([FromBody] int userId)
    {
        var subscriberUsername = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        
        await _subscriberService.Unsubscribe(subscriberUsername!, userId);
    }
    
    [HttpPost("search")]
    public async Task<List<GetUsersResponse>> SearchUsers([FromQuery] string q)
    {
        return await _subscriberService.SearchUsers(q);
    }
}