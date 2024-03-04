namespace Chapters.Entities;

public class UserChapter : BaseEntity<int>
{
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; } = default!;
    
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public int UserRating { get; set; }
    public bool IsRead { get; set; }
    public DateTime ReadTime { get; set; }
}