using Security.Dto;

namespace Security.Service.Abstractions;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(UserSignUpDto userSignUpDto, string role = "User");
    Task<TokenDto> LoginAsync(UserLoginDto userLoginDto);
    Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto);
    Task ConfirmEmailRequest();
    Task ConfirmEmail(Guid userId, string confirmationToken);
    Task ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    Task ResetPassword(ResetPasswordDto resetPasswordDto);
    Task ChangePassword(ChangePasswordDto changePasswordDto);
}