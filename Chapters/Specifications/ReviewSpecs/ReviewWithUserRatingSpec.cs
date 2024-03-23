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

    public ReviewWithUserRatingSpec(string author)
    {
        Query
            .Include(r => r.Author)
            .Where(r => r.Author.Username == author)
            .Include(r => r.Book)
            .Include(r => r.UserRatingReviews).ThenInclude(ur => ur.User)
            .OrderByDescending(r => r.CreatedAt);
    }
}