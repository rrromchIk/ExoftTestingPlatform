using TestingApi.Models.Test;

namespace TestingApi.Models.TestTemplate;

public class QuestionsPoolTemplate : BaseEntity
{
    public Guid TestTemplateId { get; set; }
    public TestTemplate TestTemplate { get; set; } = null!;

    public string? DefaultName { get; set; }
    public int? NumOfQuestionsToBeGeneratedRestriction { get; set; }
    public GenerationStrategy? GenerationStrategyRestriction { get; set; }
    public ICollection<QuestionTemplate> QuestionsTemplates { get; set; } = null!;
    public ICollection<QuestionsPool> QuestionsPoolsFromTmpl { get; set; } = null!;
}