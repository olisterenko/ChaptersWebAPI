using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSpecs;

public sealed class UserSpec : Specification<User>, ISingleResultSpecification<User>
{
    public UserSpec(int userId)
    {
        Query
            .Where(u => u.Id == userId);
    }
    
    public UserSpec(string username)
    {
        Query
            .Where(x => x.Username == username);
    }
}