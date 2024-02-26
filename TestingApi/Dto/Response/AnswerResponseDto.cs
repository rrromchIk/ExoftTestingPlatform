namespace TestingApi.Dto.Response;

public class AnswerResponseDto : BaseResponseDto
{
    public Guid QuestionId { get; set; }
    
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }
}