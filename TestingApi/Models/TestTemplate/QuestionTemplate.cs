namespace TestingApi.Models.TestTemplate;

public class QuestionTemplate : BaseEntity
{
    public Guid QuestionsPoolTemplateId { get; set; }
    public QuestionsPoolTemplate QuestionsPoolTemplate { get; set; } = null!;
    public int? MaxScoreRestriction { get; set; }
    public ICollection<AnswerTemplate> AnswerTemplates { get; set; } = null!;
}