namespace Chapters.Dto.Requests;

public sealed class MarkChapterRequest{
    public required string Username { get; set; }
    public int ChapterId { get; set; }
}