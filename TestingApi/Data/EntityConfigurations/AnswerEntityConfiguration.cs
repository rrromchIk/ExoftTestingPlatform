using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;
using TestingApi.Models.TestTemplate;

namespace TestingApi.Data.EntityConfigurations;

public class AnswerEntityConfiguration : BaseEntityConfiguration<Answer>
{
    public override void Configure(EntityTypeBuilder<Answer> builder)
    {
        base.Configure(builder);
        
        builder.Property(qt => qt.TemplateId).IsRequired(false);
    }
    
}