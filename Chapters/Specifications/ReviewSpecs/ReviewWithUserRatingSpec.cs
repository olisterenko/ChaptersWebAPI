using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.ReviewSpecs;

public sealed class ReviewWithUserRatingSpec : Specification<Review>
{
    public ReviewWithUserRatingSpec(int bookId)
    {
        Query
            .Include(r => r.Author)
            .Include(r => r.UserRatingReviews).ThenInclude(ur => ur.User)
            .Where(r => r.BookId == bookId)
            .OrderBy(r => r.CreatedAt);
    }
}