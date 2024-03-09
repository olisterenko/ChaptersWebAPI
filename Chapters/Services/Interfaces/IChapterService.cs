﻿using Chapters.Dto.Requests;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IChapterService
{
    Task<List<GetChapterResponse>> GetChapters(GetChaptersRequest chaptersRequest);
}