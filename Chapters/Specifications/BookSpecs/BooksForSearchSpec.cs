using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.BookSpecs;

public sealed class BooksForSearchSpec : Specification<Book>
{
    public BooksForSearchSpec(string q)
    {
        Query
            .Where(b => b.Title.ToLower().Contains(q.ToLower()))
            .OrderByDescending(x => x.Rating)
            .Include(x => x.UserBooks).ThenInclude(ub => ub.User);
    }
}