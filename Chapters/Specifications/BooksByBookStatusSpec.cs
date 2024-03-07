using Ardalis.Specification;
using Chapters.Entities;
using Chapters.Enums;

namespace Chapters.Specifications;

public sealed class BooksByBookStatusSpec : Specification<Book>
{
    public BooksByBookStatusSpec(BookStatus bookStatus)
    {
        Query
            .Include(x => x.UserBooks)
            .Where(x => x.UserBooks.Any(b => b.BookStatus == bookStatus));
    }
}