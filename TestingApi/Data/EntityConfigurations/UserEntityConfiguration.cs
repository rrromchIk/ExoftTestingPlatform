using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations; 

public class UserEntityConfiguration : BaseEntityConfiguration<User> {
    public override void Configure(EntityTypeBuilder<User> builder) {
        base.Configure(builder);
        
        builder
            .HasIndex(u => u.Login)
            .IsUnique();
    }
}