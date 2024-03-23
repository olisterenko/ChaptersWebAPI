namespace Chapters.Dto.Responses;

public record GetUserCommentResponse(
    int Id,
    int AuthorId,
    string AuthorUsername,
    string Text,
    int Rating,
    int UserRating,
    DateTimeOffset CreatedAt,
    int ChapterId,
    string ChapterTitle,
    int BookId,
    string BookTitle
);
