using Chapters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.Infrastructure.EntityConfigurations;

public class UserChapterConfig : IEntityTypeConfiguration<UserChapter>
{
    public void Configure(EntityTypeBuilder<UserChapter> builder)
    {
        builder
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserChapters)
            .HasForeignKey(uc => uc.UserId);
        
        builder
            .HasOne(uc => uc.Chapter)
            .WithMany(ch => ch.UserChapters)
            .HasForeignKey(uc => uc.ChapterId);

        builder.ToTable("UserChapters");
    }
}