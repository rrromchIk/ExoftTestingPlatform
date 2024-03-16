using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingApi.Constants;
using TestingApi.Models;

namespace TestingApi.Data.EntityConfigurations;

public class SuperAdminSeedingConfiguration : IEntityTypeConfiguration<User> 
{
    private readonly SuperAdminSeedData _superAdminSeedData;

    public SuperAdminSeedingConfiguration(SuperAdminSeedData superAdminSeedData)
    {
        _superAdminSeedData = superAdminSeedData;
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        var appUser = new User {
            Id = Guid.Parse(_superAdminSeedData.SuperAdminId),
            UserRole = (UserRole)Enum.Parse(typeof(UserRole), _superAdminSeedData.SuperAdminRole, true),
            Email = _superAdminSeedData.Email,
            EmailConfirmed = true,
            FirstName = _superAdminSeedData.FirstName,
            LastName = _superAdminSeedData.LastName,
        };
        
        builder.HasData(appUser);
    }
}