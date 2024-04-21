namespace TestingApi.Dto.AnswerDto;

public class AnswerResponseDto : BaseResponseDto
{
    public Guid QuestionId { get; set; }
    
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public Guid? TemplateId { get; set; }
}