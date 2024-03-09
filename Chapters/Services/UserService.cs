using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Exceptions;
using Chapters.Services.Interfaces;

namespace Chapters.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IRepository<User> repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task CreateUser(CreateUserRequest request)
    {
        var entity = new User
        {
            Username = request.Username,
            PasswordHash = _passwordHasher.HashPassword(request.Password)
        };
        
        if (!await _repository.TryAddUniqueAsync(entity))
        {
            throw new EntityNotFoundException<User>(0);
        }
    }
}