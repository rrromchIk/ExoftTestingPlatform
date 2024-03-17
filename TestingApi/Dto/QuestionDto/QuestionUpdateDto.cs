using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.QuestionDto;

public class QuestionUpdateDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Text can't exceed 100 characters")]
    public string Text { get; set; } = null!;
    
    [Required(ErrorMessage = "MaxScore is required")]
    [Range(1, int.MaxValue, ErrorMessage = "MaxScore must be greater than 0")]
    public int? MaxScore { get; set; }
}