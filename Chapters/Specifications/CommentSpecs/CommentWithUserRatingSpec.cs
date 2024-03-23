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

    public CommentWithUserRatingSpec(string author)
    {
        Query
            .Include(r => r.Author)
            .Where(r => r.Author.Username == author)
            .Include(c => c.Chapter).ThenInclude(ch => ch.Book)
            .Include(r => r.UserRatingComments).ThenInclude(ur => ur.User)
            .OrderByDescending(r => r.CreatedAt);
    }
}