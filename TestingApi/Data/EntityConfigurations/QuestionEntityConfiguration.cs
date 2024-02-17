using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations;

public class QuestionEntityConfiguration : BaseEntityConfiguration<Question>
{
    public override void Configure(EntityTypeBuilder<Question> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(q => q.UserAnswers)
            .WithOne(ua => ua.Question)
            .HasForeignKey(ua => ua.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}