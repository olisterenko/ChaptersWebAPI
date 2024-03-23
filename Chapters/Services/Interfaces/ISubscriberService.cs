using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface ISubscriberService
{
    Task<List<GetSubscriptionsResponse>> GetSubscriptions(string username);
    Task Subscribe(string subscriberUsername, int userId);
    Task Unsubscribe(string subscriberUsername, int userId);
}