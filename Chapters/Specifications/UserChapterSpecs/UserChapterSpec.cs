using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserChapterSpecs;

public sealed class UserChapterSpec : Specification<UserChapter>, ISingleResultSpecification<UserChapter>
{
    public UserChapterSpec(int userId, int chapterId)
    {
        Query
            .Where(userChapter => userChapter.UserId == userId && userChapter.ChapterId == chapterId);
    }
}