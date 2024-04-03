namespace TestingApi.Dto.UserDto;

public class UserResponseDto : BaseResponseDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public string ProfilePictureFilePath { get; set; } = null!;
    public string Role { get; set; } = null!;
}