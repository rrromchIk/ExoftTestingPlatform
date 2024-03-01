namespace TestingApi.Dto.UserDto;

public class UserResponseDto : BaseResponseDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
}