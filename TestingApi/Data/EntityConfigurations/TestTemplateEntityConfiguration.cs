using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations;

public class TestTemplateEntityConfiguration : BaseEntityConfiguration<TestTemplate>
{
    public override void Configure(EntityTypeBuilder<TestTemplate> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(t => t.QuestionsPoolTemplates)
            .WithOne(qp => qp.TestTemplate)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(qp => qp.TestTemplateId);

        builder.Property(tp => tp.NameRestriction).IsRequired(false);
        builder.Property(tp => tp.TestDifficultyRestriction).IsRequired(false);
        builder.Property(tp => tp.DurationRestriction).IsRequired(false);
        builder.Property(tp => tp.SubjectRestriction).IsRequired(false);
        
        builder.Property(t => t.TestDifficultyRestriction)
            .HasConversion(
                td => td.ToString(),
                s => (TestDifficulty)Enum.Parse(typeof(TestDifficulty), s)
            );
    }
}