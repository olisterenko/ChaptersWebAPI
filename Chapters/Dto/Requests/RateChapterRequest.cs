namespace Chapters.Dto.Requests;

public class RateChapterRequest
{
    public string? Username { get; set; }
    public int ChapterId { get; set; }
    public int NewRating { get; set; }
}