﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations;

public class UserTestEntityConfiguration : IEntityTypeConfiguration<UserTest>
{
    public void Configure(EntityTypeBuilder<UserTest> builder)
    {
        builder
            .HasKey(ut => new { ut.UserId, ut.TestId });
        builder
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTests)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(ut => ut.Test)
            .WithMany(t => t.UserTests)
            .HasForeignKey(ut => ut.TestId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(ut => ut.StartingTime);
        builder.HasIndex(ut => ut.UserScore);
    }
}