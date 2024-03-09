namespace Chapters.Dto.Responses;

public record GetReviewResponse(
    int Id,
    int AuthorId,
    string AuthorUsername,
    string Title,
    string Text,
    int Rating,
    int UserRating,
    DateTime CreatedAt
);