using Chapters.Enums;

namespace Chapters.Requests;

public class GetBooksRequest
{
    public string? Username { get; set; }
    public BookStatus? BookStatus { get; set; } = null;
}