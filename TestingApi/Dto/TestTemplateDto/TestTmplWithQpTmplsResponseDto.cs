using TestingApi.Dto.QuestionsPoolTemplateDto;
using TestingApi.Models;

namespace TestingApi.Dto.TestTemplateDto;

public class TestTmplWithQpTmplsResponseDto : BaseResponseDto
{
    public string TemplateName { get; set; } = null!;
    
    public string? DefaultTestDifficulty { get; set; }

    public string? DefaultSubject { get; set; }

    public int? DefaultDuration { get; set; }
    
    public ICollection<QuestionsPoolTmplResponseDto> QuestionsPoolTemplates { get; set; } = null!;

}