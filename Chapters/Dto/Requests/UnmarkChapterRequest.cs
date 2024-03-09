namespace Chapters.Dto.Requests;

public sealed class UnmarkChapterRequest
{
    public required string Username { get; set; }
    public int ChapterId { get; set; }
}