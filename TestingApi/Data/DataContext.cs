using Microsoft.EntityFrameworkCore;
using TestingApi.Data.EntityConfigurations;
using TestingApi.Models;

namespace TestingApi.Data; 

public class DataContext : DbContext {
    public DbSet<Test> Tests { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserTest> UserTests { get; set; } = null!;
    public DbSet<UserAnswer> UserAnswers { get; set; } = null!;
    
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TestEntityConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionsPoolEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserTestEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserAnswerEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}