using Chapters.Dto.Requests;

namespace Chapters.Services.Interfaces;

public interface IUserService
{
    Task CreateUser(CreateUserRequest request);
}