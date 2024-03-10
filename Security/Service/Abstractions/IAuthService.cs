using Security.Dto;

namespace Security.Service.Abstractions;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(UserSignUpDto userSignUpDto);
    Task<TokenDto> LoginAsync(UserLoginDto userLoginDto);
    Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto);
    Task<bool> VerifyEmail(Guid userId, string confirmationToken);
}