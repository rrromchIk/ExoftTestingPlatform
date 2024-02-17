using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations; 

public class QuestionsPoolEntityConfiguration : IEntityTypeConfiguration<QuestionsPool> {
    public void Configure(EntityTypeBuilder<QuestionsPool> builder) {
        builder
            .HasMany(qp => qp.Questions)
            .WithOne(q => q.QuestionsPool)
            .HasForeignKey(q => q.QuestionsPoolId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}