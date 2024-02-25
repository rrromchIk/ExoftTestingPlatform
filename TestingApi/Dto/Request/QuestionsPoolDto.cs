using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.Request;

public class QuestionsPoolDto
{
    [Required(ErrorMessage = "TestId is required")]
    public Guid TestId { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "NumOfQuestionsToBeGenerated is required")]
    [Range(1, int.MaxValue, ErrorMessage = "NumOfQuestionsToBeGenerated must be greater than 0")]
    public int NumOfQuestionsToBeGenerated { get; set; }
    
    [Required(ErrorMessage = "GenerationStrategy is required")]
    [RegularExpression(
        @"^(?i)(sequentially|randomly)$",
        ErrorMessage = "GenerationStrategy must be: sequentially or randomly (case-insensitive)"
    )]
    public string GenerationStrategy { get; set; } = null!;
}