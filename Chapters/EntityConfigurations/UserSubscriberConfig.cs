﻿using Chapters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapters.EntityConfigurations;

public class UserSubscriberConfig : IEntityTypeConfiguration<UserSubscriber>
{
    public void Configure(EntityTypeBuilder<UserSubscriber> builder)
    {
        builder
            .HasOne(e => e.User)
            .WithMany(u => u.UserSubscribers)
            .HasForeignKey(e => e.UserId);
        
        builder.ToTable("UserSubscriber");
    }
}