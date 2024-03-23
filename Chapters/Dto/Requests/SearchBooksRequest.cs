namespace Chapters.Dto.Requests;

public class SearchBooksRequest
{
    public string? Username { get; set; }
    public required string Q { get; set; }
}