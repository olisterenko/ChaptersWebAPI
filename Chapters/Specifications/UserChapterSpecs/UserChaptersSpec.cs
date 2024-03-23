using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserChapterSpecs;

public sealed class UserChaptersSpec : Specification<UserChapter>
{
    public UserChaptersSpec(int userId, int bookId)
    {
        Query
            .Include(userChapter => userChapter.Chapter)
            .Where(userChapter => userChapter.UserId == userId && userChapter.Chapter.BookId == bookId);
    }
}