using Chapters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.Infrastructure.EntityConfigurations;

public class UserRatingReviewConfig : IEntityTypeConfiguration<UserRatingReview>
{
    public void Configure(EntityTypeBuilder<UserRatingReview> builder)
    {
        builder
            .HasOne(e => e.User)
            .WithMany(u => u.UserRatingReviews)
            .HasForeignKey(e => e.UserId);
        
        builder
            .HasOne(e => e.Review)
            .WithMany(r => r.UserRatingReviews)
            .HasForeignKey(e => e.ReviewId);

        builder.ToTable("UserRatingReview");
    }
}