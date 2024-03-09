using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Security.Data.Configurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>> {
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder) {
        builder.HasData(
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User", NormalizedName = "User".ToUpper() },
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "Admin".ToUpper() },
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "SuperAdmin", NormalizedName = "SuperAdmin".ToUpper() });
    }
}