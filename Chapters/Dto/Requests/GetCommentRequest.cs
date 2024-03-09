namespace Chapters.Dto.Requests;

public class GetCommentRequest
{
    public string? Username { get; set; }
    public int ChapterId { get; set; }
}