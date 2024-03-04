namespace Chapters.Entities;

public class UserRatingComment : BaseEntity<int>
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public int CommentId { get; set; }
    public Comment Comment { get; set; } = default!;
    
    public int UserRating { get; set; }
}