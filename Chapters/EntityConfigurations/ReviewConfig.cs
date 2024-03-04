using Chapters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.EntityConfigurations;

public class ReviewConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder
            .HasOne(x => x.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BookId);
        
        builder
            .HasOne(x => x.Author)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.AuthorId);
        
        builder.ToTable("Review");
    }
}