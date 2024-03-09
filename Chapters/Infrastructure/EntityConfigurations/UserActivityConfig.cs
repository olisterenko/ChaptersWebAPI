using Chapters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.Infrastructure.EntityConfigurations;

public class UserActivityConfig : IEntityTypeConfiguration<UserActivity>
{
    public void Configure(EntityTypeBuilder<UserActivity> builder)
    {
        builder
            .HasOne(u => u.User)
            .WithMany(u => u.UserActivities)
            .HasForeignKey(u => u.UserId);

        builder.ToTable("UserActivity");
    }
}