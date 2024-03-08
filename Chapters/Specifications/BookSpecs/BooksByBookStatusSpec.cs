using Ardalis.Specification;
using Chapters.Domain.Entities;
using Chapters.Domain.Enums;

namespace Chapters.Specifications.BookSpecs;

public sealed class BooksByBookStatusSpec : Specification<Book>
{
    public BooksByBookStatusSpec(BookStatus bookStatus)
    {
        Query
            .Include(x => x.UserBooks)
            .Where(x => x.UserBooks.Any(b => b.BookStatus == bookStatus));
    }
}