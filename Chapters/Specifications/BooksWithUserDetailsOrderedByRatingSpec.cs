using Ardalis.Specification;
using Chapters.Entities;

namespace Chapters.Specifications;

public sealed class BooksWithUserDetailsOrderedByRatingSpec : Specification<Book>
{
    public BooksWithUserDetailsOrderedByRatingSpec()
    {
        Query
            .OrderByDescending(x => x.Rating)
            .Include(x => x.UserBooks);
    }
}