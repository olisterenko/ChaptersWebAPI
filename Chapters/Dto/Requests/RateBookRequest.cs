namespace Chapters.Dto.Requests;

public class RateBookRequest
{
    public string? Username { get; set; }
    public int BookId { get; set; }
    public int NewRating { get; set; }
}