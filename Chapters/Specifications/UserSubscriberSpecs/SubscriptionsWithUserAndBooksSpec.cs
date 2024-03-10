using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSubscriberSpecs;

public sealed class SubscriptionsWithUserAndBooksSpec : Specification<UserSubscriber>
{
    public SubscriptionsWithUserAndBooksSpec(int subscriberId)
    {
        Query
            .Where(us => us.SubscriberId == subscriberId)
            .Include(us => us.User).ThenInclude(u => u.UserBooks);
    }
}