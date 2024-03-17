using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestingApi.Constants;
using TestingApi.Data.EntityConfigurations;
using TestingApi.Models;
using TestingApi.Models.Test;
using TestingApi.Models.TestTemplate;
using TestingApi.Models.User;
using TestingApi.Services.Abstractions;

namespace TestingApi.Data;

public class DataContext : DbContext
{
    public DbSet<Test> Tests { get; set; } = null!;
    public DbSet<QuestionsPool> QuestionsPools { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserTest> UserTests { get; set; } = null!;
    public DbSet<UserAnswer> UserAnswers { get; set; } = null!;
    public DbSet<TestTemplate> TestTemplates { get; set; } = null!;
    public DbSet<QuestionsPoolTemplate> QuestionsPoolTemplates { get; set; } = null!;
    public DbSet<QuestionTemplate> QuestionTemplates { get; set; } = null!;
    public DbSet<AnswerTemplate> AnswerTemplates { get; set; } = null!;

    private readonly ICurrentUserService _currentUserService;
    private readonly SuperAdminSeedData _superAdminSeedData;
    public DataContext(DbContextOptions<DataContext> options, ICurrentUserService currentUserService,
        IOptions<SuperAdminSeedData> superAdminSeedData) : base(options)
    {
        _currentUserService = currentUserService;
        _superAdminSeedData = superAdminSeedData.Value;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TestEntityConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionsPoolEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserTestEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserAnswerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TestTemplateEntityConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionPoolTemplateEntityConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionTemplateEntityConfiguration());
        modelBuilder.ApplyConfiguration(new AnswerTemplateEntityConfiguration());
        modelBuilder.ApplyConfiguration(new AnswerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SuperAdminSeedingConfiguration(_superAdminSeedData));
        base.OnModelCreating(modelBuilder);
    }
    
    public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        baseEntity.ModifiedTimestamp = DateTime.Now;
                        baseEntity.ModifiedBy = Guid.Parse(_currentUserService.UserId);
                        break;

                    case EntityState.Added:
                        baseEntity.CreatedTimestamp = DateTime.Now;
                        if (_currentUserService.UserId != null)
                            baseEntity.CreatedBy = Guid.Parse(_currentUserService.UserId);
                        if (baseEntity.Id == default)
                            baseEntity.Id = Guid.NewGuid();
                        break;
                }
            }
        }
    }
}