using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSpecs;

public sealed class UserWithActivitySpec : Specification<User>, ISingleResultSpecification<User>
{
    public UserWithActivitySpec(string username)
    {
        Query
            .Where(u => u.Username == username)
            .Include(u => u.UserActivities);
    }
}