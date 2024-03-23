namespace Chapters.Dto.Requests;

public class PostReviewRequest
{
    public int BookId { get; set; }
    public string Title { get; set; } = default!;
    public string Text { get; set; } = default!;
}