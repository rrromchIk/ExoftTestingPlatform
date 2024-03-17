using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;
using TestingApi.Models.TestTemplate;

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
        
        builder
            .HasMany(qp => qp.QuestionsPoolsFromTmpl)
            .WithOne(qp => qp.QuestionsPoolTemplate)
            .HasForeignKey(qp => qp.TemplateId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(qp => qp.NumOfQuestionsToBeGeneratedRestriction).IsRequired(false);
        builder.Property(qp => qp.GenerationStrategyRestriction).IsRequired(false);
        builder.Property(qp => qp.DefaultName).IsRequired(false);
    }
}