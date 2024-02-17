using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

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
    }
}