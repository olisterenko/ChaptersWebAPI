namespace Chapters.Domain.Entities;

public class Comment : BaseEntity<int>
{
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; } = default!;

    public int AuthorId { get; set; }
    public User Author { get; set; } = default!;

    public string Text { get; set; } = default!;

    public int Rating { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public List<UserRatingComment> UserRatingComments { get; set; } = [];
}