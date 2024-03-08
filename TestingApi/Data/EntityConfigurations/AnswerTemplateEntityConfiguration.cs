using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models.TestTemplate;

namespace TestingApi.Data.EntityConfigurations;

public class AnswerTemplateEntityConfiguration : BaseEntityConfiguration<AnswerTemplate>
{
    public override void Configure(EntityTypeBuilder<AnswerTemplate> builder)
    {
        base.Configure(builder);
        
        builder.Property(qt => qt.DefaultText).IsRequired(false);
    }
}