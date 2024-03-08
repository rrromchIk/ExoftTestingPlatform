namespace TestingApi.Dto.QuestionsPoolTemplateDto;

public class QuestionsPoolTemplateResponseDto : BaseResponseDto
{
    public Guid TestTemplateId { get; set; }
    public string? DefaultName { get; set; }
    public int? NumOfQuestionsToBeGeneratedRestriction { get; set; }
    public string? GenerationStrategyRestriction { get; set; }
}