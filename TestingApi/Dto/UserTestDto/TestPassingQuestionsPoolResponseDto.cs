namespace TestingApi.Dto.UserTestDto;

public class TestPassingQuestionsPoolResponseDto
{
    public Guid QuestionsPoolId { get; set; }
    public string GenerationStrategy { get; set; } = null!;
    public int NumOfQuestionsToBeGenerated { get; set; }
    public ICollection<UserQuestionResponseDto> UserQuestions { get; set; } = null!;
}