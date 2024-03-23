using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.ChapterSpecs;

public sealed class ChapterSpec : Specification<Chapter>, ISingleResultSpecification<Chapter>
{
    public ChapterSpec(int chapterId)
    {
        Query
            .Where(ch => ch.Id == chapterId);
    }
}