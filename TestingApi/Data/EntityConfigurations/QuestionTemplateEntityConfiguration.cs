using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;
using TestingApi.Models.TestTemplate;

namespace TestingApi.Data.EntityConfigurations;

public class QuestionTemplateEntityConfiguration : BaseEntityConfiguration<QuestionTemplate>
{
    public override void Configure(EntityTypeBuilder<QuestionTemplate> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(q => q.AnswerTemplates)
            .WithOne(a => a.QuestionTemplate)
            .HasForeignKey(a => a.QuestionTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(qt => qt.DefaultText).IsRequired(false);
        builder.Property(qt => qt.MaxScoreRestriction).IsRequired(false);
    }
}