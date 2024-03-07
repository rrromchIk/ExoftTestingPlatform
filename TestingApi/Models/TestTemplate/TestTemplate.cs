namespace TestingApi.Models.TestTemplate;

public class TestTemplate : BaseEntity
{
    public TestDifficulty? DefaultTestDifficulty { get; set; }

    public string? DefaultSubject { get; set; }

    public int? DefaultDuration { get; set; }

    public ICollection<QuestionsPoolTemplate> QuestionsPoolTemplates { get; set; } = null!;
}