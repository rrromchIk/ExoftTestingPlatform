using Security.Dto;
using Security.Models;

namespace Security.Service.Abstractions;

public interface ITokenGenerator
{
    public string GenerateAccessToken(ApplicationUser user);
    public string GenerateRefreshToken(ApplicationUser user);
    public Task<TokenResponseDto> RefreshAccessToken(string accessToken, string refreshToken);
}