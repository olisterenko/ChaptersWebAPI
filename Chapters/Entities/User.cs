namespace Chapters.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = default!;
    
    public string PasswordHash { get; set; } = default!;
}