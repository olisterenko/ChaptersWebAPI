﻿using System.Security.Claims;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[Route("/api/comments")]
public class CommentsController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentService _commentService;

    public CommentsController(IHttpContextAccessor httpContextAccessor, ICommentService commentService)
    {
        _httpContextAccessor = httpContextAccessor;
        _commentService = commentService;
    }

    [HttpGet("{chapterId:int}")]
    public async Task<List<GetCommentResponse>> GetComments(int chapterId)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        return await _commentService.GetComments(
            new GetCommentRequest
            {
                Username = username?.Value,
                ChapterId = chapterId
            }
        );
    }

    [Authorize]
    [HttpPost]
    public async Task PostComment(PostCommentRequest postCommentRequest)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _commentService.PostComment(username!.Value, postCommentRequest);
    }

    [HttpGet("user")]
    public async Task<List<GetUserCommentResponse>> GetUserComments([FromBody] string author)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        return await _commentService.GetUserComments(
            new GetUserCommentsRequest
            {
                Author = author,
                Username = username?.Value
            });
    }

    [Authorize]
    [HttpPost("{commentId:int}")]
    public async Task RateComment(int commentId, [FromBody] bool isPositive)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _commentService.RateComment(username!.Value, commentId, isPositive);
    }
}