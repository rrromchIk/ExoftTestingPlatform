using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.Request;

public class TestDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Subject is required")]
    [MinLength(2, ErrorMessage = "Subject must be at least 2 characters")]
    public string Subject { get; set; } = null!;

    [Required(ErrorMessage = "Duration is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Difficulty is required")]
    [RegularExpression(
        @"^(?i)(easy|medium|hard)$",
        ErrorMessage = "Difficulty must be: easy, medium or hard (case-insensitive)"
    )]
    public string Difficulty { get; set; } = null!;
}


