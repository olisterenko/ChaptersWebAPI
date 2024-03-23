using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.BookSpecs;

public sealed class BookSpec : Specification<Book>, ISingleResultSpecification<Book>
{
    public BookSpec(int bookId)
    {
        Query
            .Where(x => x.Id == bookId);
    }
}