namespace Security.Dto;

public class UserLoginResponseDto
{
    public UserResponseDto UserData { get; set; }
    public TokenDto TokensPair { get; set; }
}