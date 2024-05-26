using Chapters;
using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Exceptions;
using Chapters.Services;
using Chapters.Services.Interfaces;
using Moq;

namespace Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IRepository<User>> _mockRepository = new();
    private readonly Mock<IPasswordHasher> _mockPasswordHasher = new();
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService(_mockRepository.Object, _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task CreateUser_Successful()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest("username", "password123");
        const string hashedPassword = "hashedpassword";

        _mockPasswordHasher
            .Setup(ph => ph.HashPassword(createUserRequest.Password))
            .Returns(hashedPassword);
        _mockRepository
            .Setup(repo => repo.TryAddUniqueAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        // Act
        await _userService.CreateUser(createUserRequest);

        // Assert
        _mockRepository.Verify(
            repo => repo.TryAddUniqueAsync(
                It.Is<User>(u => u.Username == createUserRequest.Username && u.PasswordHash == hashedPassword)),
            Times.Once);
    }

    [Fact]
    public async Task CreateUser_WhenUserNotAdded_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest("username", "password123");

        const string hashedPassword = "hashedpassword";

        _mockPasswordHasher
            .Setup(ph => ph.HashPassword(createUserRequest.Password))
            .Returns(hashedPassword);
        _mockRepository
            .Setup(repo => repo.TryAddUniqueAsync(It.IsAny<User>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException<User>>(() => _userService.CreateUser(createUserRequest));
    }
}