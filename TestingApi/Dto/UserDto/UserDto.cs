using System.ComponentModel.DataAnnotations;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Models;

namespace TestingApi.Dto.UserDto;

public class UserDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    [MaxLength(25, ErrorMessage = "Name can't exceed 25 characters")]
    public string Name { get; set; } = null!;
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Surname is required")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
    public string Surname { get; set; } = null!;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "UserRole is required")]
    [EnumValue(typeof(UserRole))]
    public string UserRole { get; set; } = null!;
}