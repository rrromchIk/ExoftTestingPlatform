using TestingApi.Models;

namespace TestingApi.Dto.TestTemplateDto;

public class TestTemplateResponseDto : BaseResponseDto
{
    public string TemplateName { get; set; } = null!;
    public string? DefaultTestDifficulty { get; set; }

    public string? DefaultSubject { get; set; }

    public int? DefaultDuration { get; set; }
}