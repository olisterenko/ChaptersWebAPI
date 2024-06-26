﻿using Ardalis.Specification;
using Chapters.Domain.Entities;

namespace Chapters.Specifications.ChapterSpecs;

public sealed class ChaptersWithUserChaptersOrderedSpec : Specification<Chapter>
{
    public ChaptersWithUserChaptersOrderedSpec(int bookId)
    {
        Query
            .Include(c => c.UserChapters).ThenInclude(uc => uc.User)
            .Where(c => c.BookId == bookId)
            .OrderBy(c => c.Number);
    }
}