namespace TestingApi.Dto.AnswerTemplateDto;

public class AnswerTmplResponseDto : BaseResponseDto
{
    public Guid QuestionTemplateId { get; set; }
    public string? DefaultText { get; set; }
    public bool IsCorrectRestriction { get; set; }
}