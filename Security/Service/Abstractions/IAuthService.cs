using Security.Dto;

namespace Security.Service.Abstractions;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(UserSignUpDto userSignUpDto);
    Task<TokenDto> LoginAsync(UserLoginDto userLoginDto);
    Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto);
    Task VerifyEmail(Guid userId, string confirmationToken);
    Task ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    Task ResetPassword(ResetPasswordDto resetPasswordDto);
    Task ChangePassword(ChangePasswordDto changePasswordDto);
}