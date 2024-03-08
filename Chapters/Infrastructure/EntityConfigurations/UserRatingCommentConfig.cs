using Chapters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.Infrastructure.EntityConfigurations;

public class UserRatingCommentConfig : IEntityTypeConfiguration<UserRatingComment>
{
    public void Configure(EntityTypeBuilder<UserRatingComment> builder)
    {
        builder
            .HasOne(e => e.User)
            .WithMany(u => u.UserRatingComments)
            .HasForeignKey(e => e.UserId);
        
        builder
            .HasOne(e => e.Comment)
            .WithMany(r => r.UserRatingComments)
            .HasForeignKey(e => e.CommentId);

        builder.ToTable("UserRatingComment");
    }
}