using System.ComponentModel.DataAnnotations;
using TestingApi.Models;

namespace TestingApi.Dto.TestTemplateDto;

public class TestTemplateDto
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
}