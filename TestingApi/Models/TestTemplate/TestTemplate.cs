using TestingApi.Models.Test;

namespace TestingApi.Models.TestTemplate;

public class TestTemplate : BaseEntity
{
    public string TemplateName { get; set; } = null!;
    public TestDifficulty? DefaultTestDifficulty { get; set; }

    public string? DefaultSubject { get; set; }

    public int? DefaultDuration { get; set; }

    public ICollection<QuestionsPoolTemplate> QuestionsPoolTemplates { get; set; } = null!;
    
    public ICollection<Test.Test> TestsFromTemplate { get; set; } = null!;
}