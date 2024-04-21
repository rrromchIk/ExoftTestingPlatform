using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.QuestionDto;

public class QuestionWithAnswersDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Text is required")]
    [MaxLength(500 , ErrorMessage = "Text can't exceed 100 characters")]
    public string Text { get; set; } = null!;
    
    [Required(ErrorMessage = "MaxScore is required")]
    [Range(1, int.MaxValue, ErrorMessage = "MaxScore must be greater than 0")]
    public int? MaxScore { get; set; }
    public Guid? TemplateId { get; set; }
    public ICollection<AnswerDto.AnswerDto>? Answers { get; set; } = null!;
}