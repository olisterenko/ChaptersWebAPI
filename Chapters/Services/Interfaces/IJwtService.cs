using System.Security.Claims;

namespace Chapters.Services.Interfaces;

public interface IJwtService
{
    string GetToken(Claim[] Claims);
    
}