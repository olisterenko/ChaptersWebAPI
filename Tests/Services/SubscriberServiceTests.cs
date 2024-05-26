using Chapters;
using Chapters.Domain.Entities;
using Chapters.Domain.Enums;
using Chapters.Services;
using Chapters.Specifications.UserSpecs;
using Chapters.Specifications.UserSubscriberSpecs;
using Moq;

namespace Tests.Services;

public class SubscriberServiceTests
{
    private readonly Mock<IRepository<UserSubscriber>> _mockUserSubscriberRepository = new();
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly SubscriberService _subscriberService;

    public SubscriberServiceTests()
    {
        _subscriberService = new SubscriberService(
            _mockUserSubscriberRepository.Object,
            _mockUserRepository.Object
        );
    }

    [Fact]
    public async Task GetSubscriptions_ShouldReturnSubscriptionList()
    {
        // Arrange
        const string username = nameof(username);
        var user = new User
        {
            Id = 1,
            Username = username,
            UserBooks = []
        };

        var subscriptions = new List<UserSubscriber>
        {
            new()
            {
                Id = 1,
                UserId = 2,
                User = new User
                {
                    Id = 2, Username = "user2",
                    UserBooks =
                    [
                        new UserBook { BookStatus = BookStatus.Finished },
                        new UserBook { BookStatus = BookStatus.Finished }
                    ]
                }
            },
            new()
            {
                Id = 2,
                UserId = 3,
                User = new User
                {
                    Id = 3, Username = "user3",
                    UserBooks =
                    [
                        new UserBook { BookStatus = BookStatus.Finished }
                    ]
                }
            }
        };

        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockUserSubscriberRepository
            .Setup(repo => repo.ListAsync(It.IsAny<SubscriptionsWithUserAndBooksSpec>(), CancellationToken.None))
            .ReturnsAsync(subscriptions);

        // Act
        var result = await _subscriberService.GetSubscriptions(username);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(2, result[0].NumberOfBooks);
        Assert.Equal(1, result[1].NumberOfBooks);
    }

    [Fact]
    public async Task Subscribe_ShouldAddUserSubscriber()
    {
        // Arrange
        const string subscriberUsername = nameof(subscriberUsername);
        const int userId = 2;

        var subscriber = new User { Id = 1, Username = subscriberUsername };

        _mockUserRepository
            .Setup(repo => repo.FirstAsync(It.IsAny<UserSpec>(), CancellationToken.None))
            .ReturnsAsync(subscriber);

        // Act
        await _subscriberService.Subscribe(subscriberUsername, userId);

        // Assert
        _mockUserSubscriberRepository.Verify(
            repo => repo.AddAsync(
                It.Is<UserSubscriber>(us => us.SubscriberId == 1 && us.UserId == 2),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task SearchUsers_ShouldReturnUserList()
    {
        // Arrange
        const string searchQuery = "test";
        var users = new List<User>
        {
            new()
            {
                Id = 1, Username = "username1", UserBooks =
                [
                    new UserBook { BookStatus = BookStatus.Finished }
                ]
            },
            new()
            {
                Id = 2, Username = "username2", UserBooks =
                [
                    new UserBook { BookStatus = BookStatus.Finished },
                    new UserBook { BookStatus = BookStatus.Finished }
                ]
            }
        };

        _mockUserRepository
            .Setup(repo => repo.ListAsync(It.IsAny<UsersForSearchSpec>(), CancellationToken.None))
            .ReturnsAsync(users);

        // Act
        var result = await _subscriberService.SearchUsers(searchQuery);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].NumberOfBooks);
        Assert.Equal(2, result[1].NumberOfBooks);
    }
}