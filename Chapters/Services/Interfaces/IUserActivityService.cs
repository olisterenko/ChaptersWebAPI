using Chapters.Domain.Enums;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IUserActivityService
{
    Task<List<GetUserActivityResponse>> GetUserActivities(string username);
    Task<List<GetUserActivityResponse>> GetSubscriptionsActivities(string username);
    Task SaveChangeStatusActivity(int userId, int bookId, BookStatus bookStatus);
    Task SaveRateBookActivity(int userId, int bookId, int rating);
    Task SaveRateChapterActivity(int userId, int chapterId, int rating);
    Task SaveReadChapterActivity(int userId, int chapterId);
    Task SavePostCommentActivity(int userId, int chapterId);
    Task SavePostReviewActivity(int userId, int bookId);
}