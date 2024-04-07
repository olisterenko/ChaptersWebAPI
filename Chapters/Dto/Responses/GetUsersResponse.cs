namespace Chapters.Dto.Responses;

public record GetUsersResponse(
    int UserId,
    string Username,
    int NumberOfBooks);