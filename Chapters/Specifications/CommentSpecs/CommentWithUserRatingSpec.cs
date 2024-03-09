using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.CommentSpecs;

public sealed class CommentWithUserRatingSpec : Specification<Comment>
{
    public CommentWithUserRatingSpec(int chapterId)
    {
        Query
            .Include(r => r.Author)
            .Include(r => r.UserRatingComments).ThenInclude(ur => ur.User)
            .Where(r => r.ChapterId == chapterId)
            .OrderBy(r => r.CreatedAt);
    }
}