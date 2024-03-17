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
    }
}