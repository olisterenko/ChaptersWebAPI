using Chapters.Enums;

namespace Chapters.Responses;

public sealed record GetBookResponse(
    int Id,
    string Title,
    string Author,
    double Rating,
    int YearWritten,
    string? Cover,
    BookStatus BookStatus,
    int UserRating,
    List<GetCompactChapterResponse> Chapters
);