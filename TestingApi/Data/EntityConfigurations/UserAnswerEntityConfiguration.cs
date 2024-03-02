using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations;

public class UserAnswerEntityConfiguration : IEntityTypeConfiguration<UserAnswer>
{
    public void Configure(EntityTypeBuilder<UserAnswer> builder)
    {
        builder.HasKey(uqa => new { uqa.UserId, uqa.QuestionId, uqa.AnswerId });
        
        builder
            .HasOne(ua => ua.Answer)
            .WithMany()
            .HasForeignKey(ua => ua.AnswerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(ua => ua.User)
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}