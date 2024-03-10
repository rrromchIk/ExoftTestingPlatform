using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Security.Settings;

namespace Security.Data.Configurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    private readonly SuperAdminSeedData _superAdminSeedData;

    public RoleEntityConfiguration(SuperAdminSeedData superAdminSeedData) {
        _superAdminSeedData = superAdminSeedData;
    }
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder) {
        builder.HasData(
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User", NormalizedName = "User".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()},
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "Admin".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole<Guid> { Id = Guid.Parse(_superAdminSeedData.SuperAdminRoleId), Name = "SuperAdmin", NormalizedName = "SuperAdmin".ToUpper(),
                ConcurrencyStamp = _superAdminSeedData.SuperAdminRoleId});
    }
}