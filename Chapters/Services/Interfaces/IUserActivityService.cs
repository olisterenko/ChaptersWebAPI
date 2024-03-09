using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IUserActivityService
{
    Task<List<GetUserActivityResponse>> GetUserActivities(string username);
}