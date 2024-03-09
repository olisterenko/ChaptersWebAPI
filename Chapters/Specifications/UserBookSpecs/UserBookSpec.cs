using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserBookSpecs;

public sealed class UserBookSpec : Specification<UserBook>, ISingleResultSpecification<UserBook>
{
    public UserBookSpec(int userId, int bookId)
    {
        Query
            .Where(userBook => userBook.UserId == userId && userBook.BookId == bookId);
    }
}