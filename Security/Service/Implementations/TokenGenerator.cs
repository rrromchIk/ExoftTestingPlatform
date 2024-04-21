using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
    
    public async Task<string> GenerateAccessToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
       
        var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var claimsIdentity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
            }
        );
        
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
        claimsIdentity.AddClaims(roleClaims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _authSettings.Issuer,
            Audience = _authSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Subject = claimsIdentity,
            Expires = DateTime.Now.AddMinutes(_authSettings.AccessTokenExpirationMinutes)
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
}