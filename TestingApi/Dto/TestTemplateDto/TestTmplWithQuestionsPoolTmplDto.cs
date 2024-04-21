using System.ComponentModel.DataAnnotations;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Models.Test;


namespace TestingApi.Dto.TestTemplateDto;

public class TestTmplWithQuestionsPoolTmplDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Template name is required")]
    [MaxLength(25, ErrorMessage = "Name can't exceed 25 characters")]
    public string TemplateName { get; set; } = null!;
    
    [EnumValue(typeof(TestDifficulty))]
    public string? DefaultTestDifficulty { get; set; }

    public string? DefaultSubject { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
    public int? DefaultDuration { get; set; }
    
    public ICollection<QuestionsPoolTemplateDto.QuestionsPoolTmplDto>? QuestionsPoolTemplates { get; set; } = null!;

}