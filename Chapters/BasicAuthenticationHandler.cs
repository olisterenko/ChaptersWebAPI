using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Ardalis.Specification;
using Chapters.Domain.Entities;
using Chapters.Exceptions;
using Chapters.Services.Interfaces;
using Chapters.Specifications;
using Chapters.Specifications.UserSpecs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Chapters;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRepository<User> _userRepository;

    [Obsolete("Obsolete")]
    public BasicAuthenticationHandler(IPasswordHasher passwordHasher, IRepository<User> userRepository,
        IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        string username;
        string password;

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            username = credentials[0];
            password = credentials[1];
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        User user;
        try
        {
            user = await _userRepository.FirstAsync(new UserSpec(username));
        }
        catch (EntityNotFoundException<User>)
        {
            return AuthenticateResult.Fail("Invalid user or password");

        }

        if (_passwordHasher.VerifyHashedPassword(user.PasswordHash, password))
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        else
        {
            return AuthenticateResult.Fail("Invalid Username or Password");
        }
    }
}