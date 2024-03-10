using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Security.Models;
using Security.Settings;

namespace Security.Data.Configurations;

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private readonly SuperAdminSeedData _superAdminSeedData;

    public ApplicationUserEntityConfiguration(SuperAdminSeedData superAdminSeedData)
    {
        _superAdminSeedData = superAdminSeedData;
    }
    
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var appUser = new ApplicationUser {
            Id = Guid.Parse(_superAdminSeedData.SuperAdminId),
            Email = _superAdminSeedData.Email,
            EmailConfirmed = true,
            FirstName = _superAdminSeedData.FirstName,
            LastName = _superAdminSeedData.LastName,
            UserName = _superAdminSeedData.Email,
            NormalizedUserName = _superAdminSeedData.Email.ToUpper()
        };

        var ph = new PasswordHasher<ApplicationUser>();
        appUser.PasswordHash = ph.HashPassword(appUser, _superAdminSeedData.Password);

        builder.HasData(appUser);
    }
}