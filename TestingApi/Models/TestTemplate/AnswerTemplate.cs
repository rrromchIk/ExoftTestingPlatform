namespace TestingApi.Models.TestTemplate;

public class AnswerTemplate : BaseEntity
{
    public Guid QuestionTemplateId { get; set; }
    public QuestionTemplate QuestionTemplate { get; set; } = null!;
    public bool IsCorrectRestriction { get; set; }
}
