using System.ComponentModel.DataAnnotations;
using TestingApi.Models;
using TestingApi.Dto.QuestionsPoolTemplateDto;


namespace TestingApi.Dto.TestTemplateDto;

public class TestTmplWithQuestionsPoolTmplDto
{
    public string TemplateName { get; set; } = null!;
    
    [RegularExpression(
        @"^(?i)(easy|medium|hard)$",
        ErrorMessage = "Difficulty must be: easy, medium or hard (case-insensitive)"
    )]
    public string? DefaultTestDifficulty { get; set; }

    public string? DefaultSubject { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
    public int? DefaultDuration { get; set; }
    
    public ICollection<QuestionsPoolTemplateDto.QuestionsPoolTmplDto> QuestionsPoolTemplates { get; set; } = null!;

}