namespace Chapters.Dto.Responses;

public record GetUserActivityResponse(
    int Id,
    int UserId,
    string Username,
    string Text,
    DateTimeOffset CreatedAt
);