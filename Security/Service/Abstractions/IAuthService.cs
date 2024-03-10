using Security.Dto;

namespace Security.Service.Abstractions;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(UserSignUpDto userSignUpDto);
    Task<TokenDto> LoginAsync(UserLoginDto userLoginDto);
    public Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto);
}