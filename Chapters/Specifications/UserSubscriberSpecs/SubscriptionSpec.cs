using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSubscriberSpecs;

public sealed class SubscriptionSpec : Specification<UserSubscriber>, ISingleResultSpecification<UserSubscriber>
{
    public SubscriptionSpec(int subscriberId, int userId)
    {
        Query
            .Where(us => us.SubscriberId == subscriberId && us.UserId == userId);
    }
}