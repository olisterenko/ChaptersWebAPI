namespace Chapters.Domain.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = default!;
    
    public string PasswordHash { get; set; } = default!;
    
    public List<UserBook> UserBooks { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<UserRatingReview> UserRatingReviews { get; set; } = new();
    public List<UserRatingComment> UserRatingComments { get; set; } = new();
    public List<UserChapter> UserChapters { get; set; } = new();
    public List<UserSubscriber> UserSubscribers { get; set; } = new();
}