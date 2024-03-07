using Ardalis.Specification;
using Chapters.Entities;

namespace Chapters.Specifications;

public sealed class BooksOrderedByRatingSpec : Specification<Book>
{
    public BooksOrderedByRatingSpec()
    {
        Query.OrderByDescending(x => x.Rating);
    }
}