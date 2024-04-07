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
    private readonly IRepository<User> _userRepository;

    public SubscriberService(
        IRepository<UserSubscriber> userSubscriberRepository,
        IRepository<User> userRepository)
    {
        _userSubscriberRepository = userSubscriberRepository;
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

    public async Task Subscribe(string subscriberUsername, int userId)
    {
        var subscriber = await _userRepository.FirstAsync(new UserSpec(subscriberUsername));

        var userSubscriber = new UserSubscriber
        {
            SubscriberId = subscriber.Id,
            UserId = userId
        };

        await _userSubscriberRepository.AddAsync(userSubscriber);
    }

    public async Task Unsubscribe(string subscriberUsername, int userId)
    {
        var subscriber = await _userRepository.FirstAsync(new UserSpec(subscriberUsername));

        await _userSubscriberRepository.DeleteRangeAsync(new SubscriptionSpec(subscriber.Id, userId));
    }

    public async Task<List<GetUsersResponse>> SearchUsers(string q)
    {
       var users = await _userRepository.ListAsync(new UsersForSearchSpec(q));

       return users
           .Select(user => new GetUsersResponse(
               UserId: user.Id,
               Username: user.Username,
               NumberOfBooks: user.UserBooks
                   .Where(ub => ub.BookStatus == BookStatus.Finished)
                   .ToList()
                   .Count
           ))
           .ToList();
    }
}