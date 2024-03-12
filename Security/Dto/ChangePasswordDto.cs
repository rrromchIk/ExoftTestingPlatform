using System.ComponentModel.DataAnnotations;

namespace Security.Dto;

public class ChangePasswordDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "NewPassword must be at least 8 characters long and include at least one uppercase letter," +
                       " one lowercase letter, one digit, and one special character."
    )]
    public string NewPassword { get; set; } = null!;
}