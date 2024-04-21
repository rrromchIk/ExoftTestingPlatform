namespace TestingApi.Dto.UserQuestionDto;

public class QuestionsPoolDetailsDto
{
    public Guid QuestionsPoolId { get; set; }
    public string GenerationStrategy { get; set; } = null!;
    public int NumOfQuestionsToBeGenerated { get; set; }
    public ICollection<Guid> QuestionsId { get; set; } = null!;
}