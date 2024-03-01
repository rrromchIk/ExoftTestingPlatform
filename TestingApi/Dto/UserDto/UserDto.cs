using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.UserDto;

public class UserDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "Surname is required")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    public string Surname { get; set; } = null!;
    
    [Required(ErrorMessage = "Name is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "UserRole is required")]
    public string UserRole { get; set; } = null!;
}