using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.UserDto;

public class UserSignUpDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "FirstName is required")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "First Name can only contain letters, numbers, and spaces!")]
    public string FirstName { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "LastName is required")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Last Name can only contain letters, numbers, and spaces!")]
    public string LastName { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter," +
                       " one lowercase letter, one digit, and one special character.")]
    public string Password { get; set; } = null!;
}
