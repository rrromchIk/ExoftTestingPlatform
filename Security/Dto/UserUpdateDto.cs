using System.ComponentModel.DataAnnotations;

namespace Security.Dto;

public class UserUpdateDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "FirstName is required")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "First Name can only contain letters, numbers, and spaces!")]
    public string FirstName { get; set; } = null!;
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "LastName is required")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Last Name can only contain letters, numbers, and spaces!")]
    public string LastName { get; set; } = null!;
}