using System.ComponentModel.DataAnnotations;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Models.Test;

namespace TestingApi.Dto.TestDto;

public class TestWithQuestionsPoolsDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
    public string Name { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Subject is required")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
    public string Subject { get; set; } = null!;

    [Required(ErrorMessage = "Duration is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
    public int? Duration { get; set; }

    [Required(ErrorMessage = "Difficulty is required")]
    [EnumValue(typeof(TestDifficulty))]
    public string Difficulty { get; set; } = null!;

    public Guid? TemplateId { get; set; }
    
    public ICollection<QuestionsPoolDto.QuestionsPoolDto>? QuestionsPools { get; set; } = null!;
}


