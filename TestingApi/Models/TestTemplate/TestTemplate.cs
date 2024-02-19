namespace TestingApi.Models;


public class TestTemplate : BaseEntity
{
    public string? NameRestriction { get; set; } = null!;

    public TestDifficulty? TestDifficultyRestriction { get; set; }

    public string? SubjectRestriction { get; set; } = null!;

    public int? DurationRestriction { get; set; }

    public ICollection<QuestionsPoolTemplate> QuestionsPoolTemplates { get; set; } = null!;
}