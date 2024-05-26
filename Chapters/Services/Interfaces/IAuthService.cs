namespace Chapters.Services.Interfaces;

public interface IAuthService
{ 
    Task<String> LoginAsync(string username, string password);
}