namespace Chapters.Domain.Entities;

public class Review : BaseEntity<int>
{
    public int BookId { get; set; }
    public Book Book { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Text { get; set; } = default!;

    public int AuthorId { get; set; }
    public User Author { get; set; } = default!;

    public int Rating { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public List<UserRatingReview> UserRatingReviews { get; set; } = [];
}