using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.UserSpecs;

public sealed class UserWithChaptersSpec : Specification<User>, ISingleResultSpecification<User>
{
    public UserWithChaptersSpec(string username)
    {
        Query
            .Where(u => u.Username == username)
            .Include(u => u.UserBooks)
            .Include(u => u.UserChapters).ThenInclude(uc => uc.Chapter);
    }
}