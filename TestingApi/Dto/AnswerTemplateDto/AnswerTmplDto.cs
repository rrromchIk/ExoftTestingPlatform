using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.AnswerTemplateDto;

public class AnswerTmplDto
{
    public string? DefaultText { get; set; }
    [Required(ErrorMessage = "IsCorrect property is required")]
    public bool? IsCorrectRestriction { get; set; }
}