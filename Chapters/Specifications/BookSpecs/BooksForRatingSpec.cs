using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.BookSpecs;

public sealed class BooksForRatingSpec : Specification<Book>
{
    public BooksForRatingSpec()
    {
        Query
            .OrderByDescending(x => x.Rating)
            .Take(500)
            .Include(x => x.UserBooks).ThenInclude(ub => ub.User);
    }
}