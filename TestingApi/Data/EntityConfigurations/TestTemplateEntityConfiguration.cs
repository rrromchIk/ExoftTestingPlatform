using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;
using TestingApi.Models.TestTemplate;

namespace TestingApi.Data.EntityConfigurations;

public class TestTemplateEntityConfiguration : BaseEntityConfiguration<TestTemplate>
{
    public override void Configure(EntityTypeBuilder<TestTemplate> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(t => t.QuestionsPoolTemplates)
            .WithOne(qp => qp.TestTemplate)
            .HasForeignKey(qp => qp.TestTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tp => tp.TemplateName).IsUnique();
        builder.Property(tp => tp.DefaultTestDifficulty).IsRequired(false);
        builder.Property(tp => tp.DefaultSubject).IsRequired(false);
        builder.Property(tp => tp.DefaultDuration).IsRequired(false);
    }
}