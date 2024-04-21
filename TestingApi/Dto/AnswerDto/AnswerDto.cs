using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.AnswerDto;

public class AnswerDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Text is required")]
    [MaxLength(300, ErrorMessage = "Text can't exceed 100 characters")]
    public string Text { get; set; } = null!;
    
    [Required(ErrorMessage = "IsCorrect property is required")]
    public bool? IsCorrect { get; set; }
    public Guid? TemplateId { get; set; }
}