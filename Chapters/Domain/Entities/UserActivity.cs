namespace Chapters.Domain.Entities;

public class UserActivity : BaseEntity<int>
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public string Text { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
}