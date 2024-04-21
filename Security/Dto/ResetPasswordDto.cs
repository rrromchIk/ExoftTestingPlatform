using System.ComponentModel.DataAnnotations;

namespace Security.Dto;

public class ResetPasswordDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "UserId is required")]
    public Guid? UserId { get; set; } = null!;
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Token is required")]
    public string Token { get; set; } = null!;
    
    [Required(AllowEmptyStrings = false)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter," +
                       " one lowercase letter, one digit, and one special character.")]
    public string NewPassword { get; set; } = null!;
}