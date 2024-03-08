using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.BookSpecs;

public sealed class BooksOrderedByRatingSpec : Specification<Book>
{
    public BooksOrderedByRatingSpec()
    {
        Query.OrderByDescending(x => x.Rating);
    }
}