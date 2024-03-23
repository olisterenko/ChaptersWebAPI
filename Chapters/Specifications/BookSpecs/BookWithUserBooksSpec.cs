using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.BookSpecs;

public sealed class BookWithUserBooksSpec : Specification<Book>, ISingleResultSpecification<Book>
{
    public BookWithUserBooksSpec(int bookId)
    {
        Query
            .Include(x => x.UserBooks)
            .Where(x => x.Id == bookId);
    }
}