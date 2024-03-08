namespace Chapters.Domain.Entities;

public class UserRatingReview : BaseEntity<int>
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public int ReviewId { get; set; }
    public Review Review { get; set; } = default!;
    
    public int UserRating { get; set; }
}