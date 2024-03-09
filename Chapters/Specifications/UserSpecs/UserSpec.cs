using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSpecs;

public sealed class UserSpec : Specification<User>, ISingleResultSpecification<User>
{
    public UserSpec(string username)
    {
        Query
            .Where(x => x.Username == username);
    }
}