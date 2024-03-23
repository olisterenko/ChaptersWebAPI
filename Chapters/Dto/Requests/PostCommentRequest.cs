namespace Chapters.Dto.Requests;

public class PostCommentRequest
{
    public int ChapterId { get; set; }
    public string Text { get; set; } = default!;
}