using Security.Dto;
using Security.Models;

namespace Security.Service.Abstractions;

public interface ITokenGenerator
{
    public Task<string> GenerateAccessToken(ApplicationUser user);
    public string GenerateRefreshToken(ApplicationUser user);
}