using Chapters.Domain.Enums;

namespace Chapters.Dto.Requests;

public class GetBooksRequest
{
    public string? Username { get; set; }
    public BookStatus? BookStatus { get; set; } = null;
}