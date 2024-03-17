using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models.TestTemplate;

namespace TestingApi.Data.EntityConfigurations;

public class AnswerTemplateEntityConfiguration : BaseEntityConfiguration<AnswerTemplate>
{
    public override void Configure(EntityTypeBuilder<AnswerTemplate> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(a => a.AnswersFromTemplate)
            .WithOne(a => a.AnswerTemplate)
            .HasForeignKey(a => a.TemplateId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property(qt => qt.DefaultText).IsRequired(false);
    }
}