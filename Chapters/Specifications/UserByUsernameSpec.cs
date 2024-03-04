using Ardalis.Specification;
using Chapters.Entities;

namespace Chapters.Specifications;

public sealed class UserByUsernameSpec : Specification<User>
{
    public UserByUsernameSpec(string username)
    {
        Query
            .Where(x => x.Username == username);
    }
}