using Chapters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.EntityConfigurations;

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AuthorId);
        
        builder
            .HasOne(c => c.Chapter)
            .WithMany(ch => ch.Comments)
            .HasForeignKey(c => c.ChapterId);

        builder.ToTable("Comment");
    }
}