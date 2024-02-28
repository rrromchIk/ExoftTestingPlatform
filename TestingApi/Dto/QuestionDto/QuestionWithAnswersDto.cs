using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.QuestionDto;

public class QuestionWithAnswersDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Text cannot exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    public string Text { get; set; } = null!;
    
    [Required(ErrorMessage = "MaxScore is required")]
    [Range(1, int.MaxValue, ErrorMessage = "MaxScore must be greater than 0")]
    public int MaxScore { get; set; }

    public ICollection<AnswerDto.AnswerDto> Answers { get; set; } = null!;
}