namespace Chapters.Requests;

public sealed class GetChaptersRequest
{
    public string? Username { get; set; }
    public int BookId { get; set; }
}