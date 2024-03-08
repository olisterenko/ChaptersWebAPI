using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSpecs;

public sealed class UserWithBooksSpec : Specification<User>
{
    public UserWithBooksSpec(string username)
    {
        Query.Include(u => u.UserBooks)
            .ThenInclude(u => u.Book)
            .ThenInclude(b => b.Chapters)
            .ThenInclude(ch => ch.UserChapters)
            .Where(u => u.Username == username);
    }
}