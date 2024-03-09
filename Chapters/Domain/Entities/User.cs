namespace Chapters.Domain.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public List<UserBook> UserBooks { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
    public List<UserRatingReview> UserRatingReviews { get; set; } = [];
    public List<UserRatingComment> UserRatingComments { get; set; } = [];
    public List<UserChapter> UserChapters { get; set; } = [];
    public List<UserSubscriber> UserSubscribers { get; set; } = [];
    public List<UserActivity> UserActivities { get; set; } = [];
}