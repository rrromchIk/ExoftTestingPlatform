using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.AnswerDto;

public class AnswerDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Text cannot exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    public string Text { get; set; } = null!;
    
    [Required(ErrorMessage = "IsCorrect property is required")]
    public bool IsCorrect { get; set; }
}