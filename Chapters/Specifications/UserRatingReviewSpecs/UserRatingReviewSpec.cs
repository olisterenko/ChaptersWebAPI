using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserRatingReviewSpecs;

public sealed class UserRatingReviewSpec : Specification<UserRatingReview>, ISingleResultSpecification<UserRatingReview>
{
    public UserRatingReviewSpec(int userId, int reviewId)
    {
        Query
            .Where(urc => urc.UserId == userId && urc.ReviewId == reviewId);
    }
}