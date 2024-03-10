namespace Chapters.Dto.Responses;

public record GetSubscriptionsResponse(
    int Id,
    int UserId,
    string Username,
    int NumberOfBooks);