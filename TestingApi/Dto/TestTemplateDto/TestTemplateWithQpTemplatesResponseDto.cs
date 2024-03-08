using TestingApi.Dto.QuestionsPoolTemplateDto;
using TestingApi.Models;

namespace TestingApi.Dto.TestTemplateDto;

public class TestTemplateWithQpTemplatesResponseDto : BaseResponseDto
{
    public string TemplateName { get; set; } = null!;
    
    public string? DefaultTestDifficulty { get; set; }

    public string? DefaultSubject { get; set; }

    public int? DefaultDuration { get; set; }
    
    public ICollection<QuestionsPoolTemplateResponseDto> QuestionsPoolTemplates { get; set; } = null!;

}