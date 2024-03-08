namespace Chapters.Responses;

public record GetCompactChapterResponse(
    int Id,
    string Title,
    double Rating,
    bool Status,
    int UserRating,
    DateTime ReadDate
);