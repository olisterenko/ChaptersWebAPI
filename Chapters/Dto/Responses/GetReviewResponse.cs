namespace Chapters.Dto.Responses;

public record GetReviewResponse(
    int Id,
    int AuthorId,
    string AuthorUsername,
    int AuthorBookRating,
    string Title,
    string Text,
    int Rating,
    int UserRating,
    DateTimeOffset CreatedAt
);