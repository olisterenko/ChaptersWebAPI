using Ardalis.Specification;
using Chapters.Entities;

namespace Chapters.Specifications;

public sealed class BookByIdSpec : Specification<Book>
{
    public BookByIdSpec(int bookId)
    {
        Query
            .Include(x => x.Chapters)
            .Where(x => x.Id == bookId);
    }
}