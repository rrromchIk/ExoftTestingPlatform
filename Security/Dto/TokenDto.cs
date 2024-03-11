using System.ComponentModel.DataAnnotations;

namespace Security.Dto;

public class TokenDto
{
    [Required(AllowEmptyStrings = false)]
    public string AccessToken { get; set; } = null!;
    
    [Required(AllowEmptyStrings = false)]
    public string RefreshToken { get; set; } = null!;
}