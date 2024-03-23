namespace Chapters.Dto.Requests;

public class GetUserReviewRequest
{
    public required string Author { get; set; }
    public string? Username { get; set; }
}