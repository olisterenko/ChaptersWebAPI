using Chapters.Domain.Enums;

namespace Chapters.Dto.Requests;

public class ChangeBookStatusRequest
{
    public string? Username { get; set; }
    public int BookId { get; set; }
    public BookStatus NewStatus { get; set; }
}