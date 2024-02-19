using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations;

public class AnswerTemplateEntityConfiguration : BaseEntityConfiguration<AnswerTemplate>
{
    public override void Configure(EntityTypeBuilder<AnswerTemplate> builder)
    {
        base.Configure(builder);

        builder.Property(at => at.TextRestriction).IsRequired(false);
        builder.Property(at => at.IsCorrectRestriction).IsRequired(false);
    }
}