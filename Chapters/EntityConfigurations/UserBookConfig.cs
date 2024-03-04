using Chapters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.EntityConfigurations;

public class UserBookConfig : IEntityTypeConfiguration<UserBook>
{
    public void Configure(EntityTypeBuilder<UserBook> builder)
    {
        builder
            .HasOne(ub => ub.Book)
            .WithMany(ub => ub.UserBooks)
            .HasForeignKey(ub => ub.BookId);
        
        builder
            .HasOne(ub => ub.User)
            .WithMany(ub => ub.UserBooks)
            .HasForeignKey(ub => ub.UserId);

        builder.ToTable("UserBooks");
    }
}