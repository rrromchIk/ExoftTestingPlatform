using System.ComponentModel.DataAnnotations;

namespace Security.Dto;

public class TokenDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Access Token is required")]
    public string AccessToken { get; set; } = null!;
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Refresh Token is required")]
    public string RefreshToken { get; set; } = null!;
}