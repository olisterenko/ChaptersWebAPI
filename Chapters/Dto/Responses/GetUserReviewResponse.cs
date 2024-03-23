namespace Chapters.Dto.Responses;

public record GetUserReviewResponse(
    int Id,
    int AuthorId,
    string AuthorUsername,
    int AuthorBookRating,
    string Title,
    string Text,
    int Rating,
    int UserRating,
    DateTimeOffset CreatedAt,
    int BookId,
    string BookTitle
);