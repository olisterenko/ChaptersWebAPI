using System.Security.Claims;
using Chapters.Domain.Entities;
using Chapters.Exceptions;
using Chapters.Services.Interfaces;
using Chapters.Specifications.UserSpecs;

namespace Chapters.Services;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IJwtService jwtService, IRepository<User> userRepository, IPasswordHasher passwordHasher)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<string> LoginAsync(string username, string password)
    {
        User user;
        try
        {
            user = await _userRepository.FirstAsync(new UserSpec(username));
        }
        catch (EntityNotFoundException<User>)
        {
            throw new EntityNotFoundException("Invalid user or password");
        }

        if (_passwordHasher.VerifyHashedPassword(user.PasswordHash, password))
        {
            var token = _jwtService.GetToken(
                [
                    new(ClaimTypes.Name, username)
                ]
            );

            return token;
        }
        
        throw new EntityNotFoundException("Invalid user or password");
    }
}