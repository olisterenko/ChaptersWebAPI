using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.BookSpecs;

public sealed class BooksForSearchSpec : Specification<Book>
{
    public BooksForSearchSpec(string q)
    {
        Query
            .Where(b => b.Title.Contains(q, StringComparison.InvariantCultureIgnoreCase))
            .OrderByDescending(x => x.Rating)
            .Take(500)
            .Include(x => x.UserBooks).ThenInclude(ub => ub.User);
    }
}