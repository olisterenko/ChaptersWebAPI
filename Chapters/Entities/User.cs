namespace Chapters.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = default!;
    
    public string PasswordHash { get; set; } = default!;
    
    public List<UserBook> UserBooks { get; set; } = default!;
    public List<Review> Reviews { get; set; } = default!;
    public List<Comment> Comments { get; set; } = default!;
    public List<UserRatingReview> UserRatingReviews { get; set; } = default!;
    public List<UserRatingComment> UserRatingComments { get; set; } = default!;
    public List<UserChapter> UserChapters { get; set; } = default!;
    public List<UserSubscriber> UserSubscribers { get; set; } = default!;
}