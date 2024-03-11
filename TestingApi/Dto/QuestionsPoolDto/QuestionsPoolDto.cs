using System.ComponentModel.DataAnnotations;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Models;

namespace TestingApi.Dto.QuestionsPoolDto;

public class QuestionsPoolDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    [MaxLength(25, ErrorMessage = "Name can't exceed 25 characters")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "NumOfQuestionsToBeGenerated is required")]
    [Range(1, int.MaxValue, ErrorMessage = "NumOfQuestionsToBeGenerated must be greater than 0")]
    public int? NumOfQuestionsToBeGenerated { get; set; }
    
    [Required(ErrorMessage = "GenerationStrategy is required")]
    [EnumValue(typeof(GenerationStrategy))]
    public string GenerationStrategy { get; set; } = null!;
}