using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.BookSpecs;

public sealed class BookWithChaptersSpec : Specification<Book>, ISingleResultSpecification<Book>
{
    public BookWithChaptersSpec(int bookId)
    {
        Query
            .Include(x => x.Chapters)
            .Where(x => x.Id == bookId);
    }
}