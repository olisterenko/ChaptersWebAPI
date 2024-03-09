using Chapters.Domain.Entities;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.UserSpecs;

namespace Chapters.Services;

public class UserActivityService : IUserActivityService
{
    private readonly IRepository<UserActivity> _userActivityRepository;
    private readonly IRepository<User> _userRepository;

    public UserActivityService(IRepository<UserActivity> userActivityRepository, IRepository<User> userRepository)
    {
        _userActivityRepository = userActivityRepository;
        _userRepository = userRepository;
    }

    public async Task<List<GetUserActivityResponse>> GetUserActivities(string username)
    {
        var user = await _userRepository.FirstAsync(new UserWithActivitySpec(username));

        return user.UserActivities
            .Select(activity => new GetUserActivityResponse(
                Id: activity.Id,
                UserId: user.Id,
                Username: username,
                Text: activity.Text,
                CreatedAt: activity.CreatedAt
            ))
            .ToList();
    }
}