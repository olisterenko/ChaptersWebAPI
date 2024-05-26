using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chapters.Options;
using Chapters.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Chapters.Services;

public class JwtService : IJwtService
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public string GetToken(Claim[] Claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Value.Secret));

        var issuer = _jwtOptions.Value.Issuer;
        var audience = _jwtOptions.Value.Audience;

        var tokenHandler = new JwtSecurityTokenHandler();
        var expires = DateTime.UtcNow.AddSeconds(_jwtOptions.Value.TokenLifetime);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
}