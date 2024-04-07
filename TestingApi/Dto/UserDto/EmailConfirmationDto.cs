using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.UserDto;

public class EmailConfirmationDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "UserId is required")]
    public Guid? UserId { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Token is required")]
    public string Token { get; set; } = null!;
}