using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models.Test;

namespace TestingApi.Data.EntityConfigurations;

public class TestEntityConfiguration : BaseEntityConfiguration<Test>
{
    public override void Configure(EntityTypeBuilder<Test> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(t => t.QuestionsPools)
            .WithOne(qp => qp.Test)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(qp => qp.TestId);

        builder
            .HasIndex(t => t.Name)
            .IsUnique();
        builder.HasIndex(t => t.CreatedTimestamp);
        builder.HasIndex(t => t.Duration);
        builder.HasIndex(t => t.ModifiedTimestamp);
        
        builder.Property(t => t.TemplateId).IsRequired(false);
    }
}