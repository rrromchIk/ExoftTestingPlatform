using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.UserQuestionDto;

public class UserQuestionDto
{
    [Required(ErrorMessage = "UserId is required")]
    public Guid? UserId { get; set; }
    
    [Required(ErrorMessage = "UserId is required")]
    public Guid? QuestionId { get; set; }
}