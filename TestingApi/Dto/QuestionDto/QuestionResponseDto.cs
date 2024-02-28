namespace TestingApi.Dto.QuestionDto;

public class QuestionResponseDto : BaseResponseDto
{
    public Guid QuestionsPoolId { get; set; }
    
    public string Text { get; set; } = null!;
    public int MaxScore { get; set; }
}