using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations;

public class QuestionPoolTemplateEntityConfiguration : BaseEntityConfiguration<QuestionsPoolTemplate>
{
    public override void Configure(EntityTypeBuilder<QuestionsPoolTemplate> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(qp => qp.QuestionsTemplates)
            .WithOne(q => q.QuestionsPoolTemplate)
            .HasForeignKey(q => q.QuestionsPoolTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(qp => qp.NameRestriction).IsRequired(false);
        builder.Property(qp => qp.NumOfQuestionsToBeGeneratedRestriction).IsRequired(false);
        builder.Property(qp => qp.GenerationStrategyRestriction).IsRequired(false);
    }
}