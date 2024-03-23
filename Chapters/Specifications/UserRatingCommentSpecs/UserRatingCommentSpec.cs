using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserRatingCommentSpecs;

public sealed class UserRatingCommentSpec : Specification<UserRatingComment>, ISingleResultSpecification<UserRatingComment>
{
    public UserRatingCommentSpec(int userId, int commentId)
    {
        Query
            .Where(urc => urc.UserId == userId && urc.CommentId == commentId);
    }
}