using Ardalis.Specification;
using Chapters.Domain.Entities;
using Chapters.Domain.Enums;

namespace Chapters.Specifications.BookSpecs;

public sealed class BooksByBookStatusSpec : Specification<Book>
{
    public BooksByBookStatusSpec(BookStatus bookStatus, string username)
    {
        Query
            .Include(b => b.UserBooks).ThenInclude(ub => ub.User)
            .Where(b => b.UserBooks.Any(ub => ub.User.Username == username && ub.BookStatus == bookStatus));
    }
}