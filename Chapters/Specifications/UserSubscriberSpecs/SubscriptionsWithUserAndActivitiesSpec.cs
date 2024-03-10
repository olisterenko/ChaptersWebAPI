using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSubscriberSpecs;

public sealed class SubscriptionsWithUserAndActivitiesSpec : Specification<UserSubscriber>
{
    public SubscriptionsWithUserAndActivitiesSpec(int subscriberId)
    {
        Query
            .Where(us => us.SubscriberId == subscriberId)
            .Include(us => us.User)
            .ThenInclude(u => u.UserActivities)
            .ThenInclude(ua => ua.User);
    }
}