using Chapters.Requests;
using Chapters.Responses;

namespace Chapters.Services.Interfaces;

public interface IChapterService
{
    Task<List<GetChapterResponse>> GetChapters(GetChaptersRequest chaptersRequest);
}