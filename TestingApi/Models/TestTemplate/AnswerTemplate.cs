namespace TestingApi.Models;

public class AnswerTemplate : BaseEntity
{
    public Guid QuestionTemplateId { get; set; }
    public QuestionTemplate QuestionTemplate { get; set; } = null!;

    public int? TextRestriction { get; set; }

    public bool? IsCorrectRestriction { get; set; }
}
