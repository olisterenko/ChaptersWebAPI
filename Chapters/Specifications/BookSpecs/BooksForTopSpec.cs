using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.BookSpecs;

public sealed class BooksForTopSpec : Specification<Book>
{
    public BooksForTopSpec()
    {
        Query
            .OrderByDescending(x => x.Rating)
            .Take(500)
            .Include(x => x.UserBooks).ThenInclude(ub => ub.User);
    }
}