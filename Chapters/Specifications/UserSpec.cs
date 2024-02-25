using Ardalis.Specification;
using Chapters.Entities;

namespace Chapters.Specifications;

public sealed class UserSpec : Specification<User>
{
    public UserSpec(int userId)
    {
        Query
            .Where(u => u.Id == userId);
    }
}