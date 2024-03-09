using Chapters.Domain.Enums;

namespace Chapters.Dto.Responses;

public sealed record GetBooksResponse(
    int Id,
    string Title,
    string Author,
    double Rating,
    string? Cover,
    BookStatus? BookStatus,
    int? UserRating);
