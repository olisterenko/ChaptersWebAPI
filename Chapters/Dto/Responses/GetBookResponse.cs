using Chapters.Domain.Enums;

namespace Chapters.Dto.Responses;

public sealed record GetBookResponse(
    int Id,
    string Title,
    string Author,
    double Rating,
    string? Cover,
    BookStatus? BookStatus,
    int? UserRating);
