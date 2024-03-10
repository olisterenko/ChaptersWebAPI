using Chapters.Domain.Entities;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.UserSpecs;
using Chapters.Specifications.UserSubscriberSpecs;

namespace Chapters.Services;

public class UserActivityService : IUserActivityService
{
    private readonly IRepository<UserActivity> _userActivityRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserSubscriber> _userSubscriberRepository;

    public UserActivityService(
        IRepository<UserActivity> userActivityRepository,
        IRepository<User> userRepository,
        IRepository<UserSubscriber> userSubscriberRepository)
    {
        _userActivityRepository = userActivityRepository;
        _userRepository = userRepository;
        _userSubscriberRepository = userSubscriberRepository;
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

    public async Task<List<GetUserActivityResponse>> GetSubscriptionsActivities(string username)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(username));

        var subscriptions = await _userSubscriberRepository
            .ListAsync(new SubscriptionsWithUserAndActivitiesSpec(user.Id));

        var subscriptionsActivities = subscriptions
            .SelectMany(subscription => subscription.User.UserActivities)
            .OrderByDescending(us => us.CreatedAt)
            .ToList();

        return subscriptionsActivities
            .Select(activity => new GetUserActivityResponse(
                Id: activity.Id,
                UserId: activity.UserId,
                Username: activity.User.Username,
                Text: activity.Text,
                CreatedAt: activity.CreatedAt
            ))
            .ToList();
    }
}