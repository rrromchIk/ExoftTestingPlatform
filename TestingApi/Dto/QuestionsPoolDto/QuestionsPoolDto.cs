using System.ComponentModel.DataAnnotations;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Models;

namespace TestingApi.Dto.QuestionsPoolDto;

public class QuestionsPoolDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "NumOfQuestionsToBeGenerated is required")]
    [Range(1, int.MaxValue, ErrorMessage = "NumOfQuestionsToBeGenerated must be greater than 0")]
    public int? NumOfQuestionsToBeGenerated { get; set; }
    
    [Required(ErrorMessage = "GenerationStrategy is required")]
    [EnumValue(typeof(GenerationStrategy))]
    public string GenerationStrategy { get; set; } = null!;
}