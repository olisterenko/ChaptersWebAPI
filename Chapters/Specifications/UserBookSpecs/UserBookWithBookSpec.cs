using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserBookSpecs;

public sealed class UserBookWithBookSpec : Specification<UserBook>, ISingleResultSpecification<UserBook>
{
    public UserBookWithBookSpec(int userId, int bookId)
    {
        Query
            .Where(userBook => userBook.UserId == userId && userBook.BookId == bookId)
            .Include(ub => ub.Book).ThenInclude(b => b.Chapters);
    }
}