﻿using Chapters.Dto.Requests;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface ICommentService
{
    Task<List<GetCommentResponse>> GetComments(GetCommentRequest commentRequest);
    Task PostComment(string username, PostCommentRequest postCommentRequest);
    Task<List<GetUserCommentResponse>> GetUserComments(GetUserCommentsRequest getUserCommentsRequest);
    Task RateComment(string username, int commentId, bool isPositive);
}