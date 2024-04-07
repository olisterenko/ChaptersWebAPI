using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSpecs;

public sealed class UsersForSearchSpec : Specification<User>
{
    public UsersForSearchSpec(string q)
    {
        Query
            .Where(u => u.Username.ToLower().Contains(q.ToLower()))
            .Include(u => u.UserBooks)
            .OrderBy(u => u.Username);
    }
}