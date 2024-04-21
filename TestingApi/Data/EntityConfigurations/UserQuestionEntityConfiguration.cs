using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations;

public class UserQuestionEntityConfiguration : IEntityTypeConfiguration<UserQuestion>
{
    public void Configure(EntityTypeBuilder<UserQuestion> builder)
    {
        builder.HasKey(uq => new { uq.UserId, uq.QuestionId });
        
        builder
            .HasOne(uq => uq.User)
            .WithMany(u => u.UserQuestions)
            .HasForeignKey(uq => uq.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(uq => uq.Question)
            .WithMany(q => q.UserQuestions)
            .HasForeignKey(uq => uq.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}