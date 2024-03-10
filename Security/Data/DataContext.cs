using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security.Data.Configurations;
using Security.Models;
using Security.Settings;

namespace Security.Data;

public class DataContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    private readonly SuperAdminSeedData _superAdminSeedData;
    public DataContext(DbContextOptions<DataContext> options,
        IOptions<SuperAdminSeedData> superAdminSeedData) : base(options)
    {
        _superAdminSeedData = superAdminSeedData.Value;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration(_superAdminSeedData));
        modelBuilder.ApplyConfiguration(new RoleEntityConfiguration(_superAdminSeedData));
        modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration(_superAdminSeedData));
    }
}