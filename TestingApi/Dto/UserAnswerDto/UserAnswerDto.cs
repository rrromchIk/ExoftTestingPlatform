using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.UserAnswerDto;

public class UserAnswerDto
{
    [Required(ErrorMessage = "UserId is required")]
    public Guid? UserId { get; set; }
    
    [Required(ErrorMessage = "QuestionId is required")]
    public Guid? QuestionId { get; set; }
    
    [Required(ErrorMessage = "AnswerId is required")]
    public Guid? AnswerId { get; set; }
}