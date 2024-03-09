using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Dto;
using Security.Models;
using Security.Service.Abstractions;
using Security.Settings;

namespace Security.Service.Implementations;

public class TokenGenerator : ITokenGenerator
{
    private const int RefreshTokenSize = 32;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthSettings _authSettings;
    private readonly ILogger<TokenGenerator> _logger;
    public TokenGenerator(UserManager<ApplicationUser> userManager, IOptions<AuthSettings> authSettings, ILogger<TokenGenerator> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _authSettings = authSettings.Value;
    }
    
    public string GenerateAccessToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        _logger.LogInformation(_authSettings.SecretKey);
        var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);
        
        var identity = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        });

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _authSettings.Issuer,
            Audience = _authSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Subject = identity,
            Expires = DateTime.Now.AddMinutes(_authSettings.Lifetime)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(ApplicationUser user)
    {
        var randomNumber = new byte[RefreshTokenSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public Task<TokenResponseDto> RefreshAccessToken(string accessToken, string refreshToken)
    {
        throw new NotImplementedException();
    }
}