namespace Chapters.Dto.Requests;

public class GetUserCommentsRequest
{
    public required string Author { get; set; }
    public string? Username { get; set; }
}