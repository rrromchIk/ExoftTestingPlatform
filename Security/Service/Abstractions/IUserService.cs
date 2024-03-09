using Security.Dto;

namespace Security.Service.Abstractions;

public interface IUserService
{
    Task<UserResponseDto> CreateUserAsync(UserSignUpDto userSignUpDto);
    Task<TokenResponseDto> Login(UserLoginDto userLoginDto);
}