namespace Chapters.Entities;

public class Review : BaseEntity<int>
{
    public int BookId { get; set; } = default!;
    public Book Book { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int AuthorId { get; set; } = default!;
    public User Author { get; set; } = default!;
    public string Text { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public int Rating { get; set; }
    
    public List<UserRatingReview> UserRatingReviews { get; set; } = default!;
}