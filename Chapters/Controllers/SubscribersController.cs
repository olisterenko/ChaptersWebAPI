﻿using System.Security.Claims;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
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
    
    [HttpPost]
    public async Task Subscribe([FromBody] int userId)
    {
        var subscriberUsername = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        
        await _subscriberService.Subscribe(subscriberUsername!, userId);
    }
    
    [HttpDelete]
    public async Task Unsubscribe([FromBody] int userId)
    {
        var subscriberUsername = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        
        await _subscriberService.Unsubscribe(subscriberUsername!, userId);
    }
}