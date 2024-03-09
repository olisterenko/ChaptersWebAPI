namespace Chapters.Dto.Requests;

public sealed class GetChaptersRequest
{
    public string? Username { get; set; }
    public int BookId { get; set; }
}