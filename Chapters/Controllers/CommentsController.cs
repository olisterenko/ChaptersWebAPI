﻿using System.Security.Claims;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
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

    [HttpGet("{chapterId}")]
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
    
    [HttpPost("{chapterId:int}")]
    public async Task PostComment(int chapterId, PostCommentRequest postCommentRequest)
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);

        await _commentService.PostComment(username!.Value, chapterId, postCommentRequest);
    }

    // TODO: получать все комментарии одного пользователя
    // TODO: менять рейтинг комментария
}
