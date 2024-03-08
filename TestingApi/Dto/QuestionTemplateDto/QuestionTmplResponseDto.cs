using TestingApi.Dto.AnswerTemplateDto;

namespace TestingApi.Dto.QuestionTemplateDto;

public class QuestionTmplResponseDto : BaseResponseDto
{
    public Guid QuestionsPoolTemplateId { get; set; }
    public string? DefaultText { get; set; }
    public int? MaxScoreRestriction { get; set; }
    public ICollection<AnswerTmplResponseDto> AnswerTemplates { get; set; } = null!;
}