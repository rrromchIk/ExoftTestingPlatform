using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models.Test;

namespace TestingApi.Data.EntityConfigurations;

public class QuestionsPoolEntityConfiguration : BaseEntityConfiguration<QuestionsPool>
{
    public override void Configure(EntityTypeBuilder<QuestionsPool> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(qp => qp.Questions)
            .WithOne(q => q.QuestionsPool)
            .HasForeignKey(q => q.QuestionsPoolId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(qp => qp.TemplateId).IsRequired(false);
    }
}