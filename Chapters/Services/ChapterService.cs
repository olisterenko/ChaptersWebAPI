﻿using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.ChapterSpecs;

namespace Chapters.Services;

public class ChapterService : IChapterService
{
    private readonly IRepository<Chapter> _chapterRepository;

    public ChapterService(IRepository<Chapter> chapterRepository)
    {
        _chapterRepository = chapterRepository;
    }

    public async Task<List<GetChapterResponse>> GetChapters(GetChaptersRequest chaptersRequest)
    {
        var chapters = await _chapterRepository
            .ListAsync(new ChaptersWithUserChaptersOrderedSpec(chaptersRequest.BookId));


        return chapters.Select(
            chapter => new GetChapterResponse(
                Id: chapter.Id,
                Number: chapter.Number,
                Title: chapter.Title,
                Rating: chapter.UserChapters.Count != 0
                    ? chapter.UserChapters.Average(userChapter => userChapter.UserRating)
                    : 0.0,
                IsRead: chapter.UserChapters
                    .FirstOrDefault(u => u.User.Username == chaptersRequest.Username)?.IsRead ?? false,
                UserRating: chapter.UserChapters
                    .FirstOrDefault(u => u.User.Username == chaptersRequest.Username)?.UserRating ?? 0
            )
        ).ToList();
    }
}