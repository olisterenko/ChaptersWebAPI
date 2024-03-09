namespace Chapters.Dto.Responses;

public sealed record GetChapterResponse(
    int Id,
    int Number,
    string Title,
    double Rating,
    bool IsRead,
    int UserRating
);