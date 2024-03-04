using Chapters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.EntityConfigurations;

public class ChapterConfig : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder
            .HasOne(ch => ch.Book)
            .WithMany(b => b.Chapters)
            .HasForeignKey(ch => ch.BookId);
        builder.ToTable("Chapter");
    }
}