using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.UserSpecs;
using Chapters.Specifications.UserSubscriberSpecs;

namespace Chapters.Services;

public class SubscriberService : ISubscriberService
{
    private readonly IRepository<UserSubscriber> _userSubscriberRepository;
    private readonly IRepository<UserBook> _userBookRepository;
    private readonly IRepository<User> _userRepository;

    public SubscriberService(
        IRepository<UserSubscriber> userSubscriberRepository,
        IRepository<UserBook> userBookRepository,
        IRepository<User> userRepository)
    {
        _userSubscriberRepository = userSubscriberRepository;
        _userBookRepository = userBookRepository;
        _userRepository = userRepository;
    }

    public async Task<List<GetSubscriptionsResponse>> GetSubscriptions(string username)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(username));
        var subscriptions = await _userSubscriberRepository.ListAsync(new SubscriptionsWithUserAndBooksSpec(user.Id));

        return subscriptions
            .Select(subscription => new GetSubscriptionsResponse(
                Id: subscription.Id,
                UserId: subscription.UserId,
                Username: subscription.User.Username,
                NumberOfBooks: subscription.User.UserBooks
                    .Where(ub => ub.BookStatus == BookStatus.Finished)
                    .ToList()
                    .Count
            ))
            .ToList();
    }
}