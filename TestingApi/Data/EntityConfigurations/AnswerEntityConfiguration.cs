using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models.Test;

namespace TestingApi.Data.EntityConfigurations;

public class AnswerEntityConfiguration : BaseEntityConfiguration<Answer>
{
    public override void Configure(EntityTypeBuilder<Answer> builder)
    {
        base.Configure(builder);
        
        builder.Property(qt => qt.TemplateId).IsRequired(false);
    }
    
}