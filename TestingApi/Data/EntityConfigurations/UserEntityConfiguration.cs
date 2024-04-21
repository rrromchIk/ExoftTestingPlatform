using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Models.User;

namespace TestingApi.Data.EntityConfigurations;

public class UserEntityConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.ProfilePictureFilePath).IsRequired(false);

        builder
            .HasIndex(u => u.Email)
            .IsUnique();
        
        builder.HasIndex(u => u.FirstName);
        builder.HasIndex(u => u.LastName);
        builder.HasIndex(u => u.CreatedTimestamp);
        builder.HasIndex(u => u.ModifiedTimestamp);
    }
}