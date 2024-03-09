namespace Chapters.Dto.Requests;

public class GetReviewsRequest
{
    public string? Username { get; set; }
    public int BookId { get; set; }
}