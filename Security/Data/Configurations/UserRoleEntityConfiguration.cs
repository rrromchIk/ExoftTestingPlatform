using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Security.Settings;

namespace Security.Data.Configurations;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    private readonly SuperAdminSeedData _superAdminSeedData;

    public UserRoleEntityConfiguration(SuperAdminSeedData superAdminSeedData) {
        _superAdminSeedData = superAdminSeedData;
    }
    
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasData(new IdentityUserRole<Guid> {
            UserId = Guid.Parse(_superAdminSeedData.SuperAdminId),
            RoleId = Guid.Parse(_superAdminSeedData.SuperAdminRoleId)
        });
    }
}