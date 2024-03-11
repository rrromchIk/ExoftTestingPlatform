using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Dto;
using Security.Exceptions;
using Security.Models;
using Security.Service.Abstractions;
using Security.Settings;

namespace Security.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly AuthSettings _authSettings;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager,
        ILogger<AuthService> logger, ITokenGenerator tokenGenerator, IOptions<AuthSettings> authSettings, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _userManager = userManager;
        _logger = logger;
        _tokenGenerator = tokenGenerator;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _authSettings = authSettings.Value;
    }

    public async Task<UserResponseDto> RegisterAsync(UserSignUpDto userSignUpDto)
    {
        var user = _mapper.Map<ApplicationUser>(userSignUpDto);
        user.UserName = user.Email;

        var result = await _userManager.CreateAsync(user, userSignUpDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        else
        {
            throw new AuthException(result.Errors.First().Description, StatusCodes.Status409Conflict);
        }
        
        var sendingResult = await SendVerificationEmail(user);
        if (!sendingResult)
            throw new AuthException("Failed to send an email", StatusCodes.Status500InternalServerError);
        
        return _mapper.Map<UserResponseDto>(userSignUpDto);
    }

    public async Task<TokenDto> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

        if (user == null)
            throw new AuthException("User with such email not found", StatusCodes.Status404NotFound);
        
        var validPassword = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        if (!validPassword)
            throw new AuthException("User unauthorized", StatusCodes.Status401Unauthorized);
        
        
        user.RefreshToken = _tokenGenerator.GenerateRefreshToken(user);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new AuthException("Unable to create refresh token", StatusCodes.Status500InternalServerError);
        }
        
        return new TokenDto
        {
            AccessToken = await _tokenGenerator.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken
        };
    }
    
    public async Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null || user.RefreshToken != tokenDto.RefreshToken)
        {
            throw new AuthException("Invalid refresh token", StatusCodes.Status401Unauthorized);
        }

        user.RefreshToken = _tokenGenerator.GenerateRefreshToken(user);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new AuthException("Unable to create refresh token", StatusCodes.Status500InternalServerError);
        }
            
        return new TokenDto
        {
            AccessToken = await _tokenGenerator.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken
        };
    }

    public async Task<bool> VerifyEmail(Guid userId, string confirmationToken)
    {
        confirmationToken = confirmationToken.Replace(' ', '+');
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new AuthException("User with such id not found", StatusCodes.Status404NotFound);
        
        var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
        return result.Succeeded;
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _authSettings.SymmetricSecurityKey,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

        return principal;
    }

    private async Task<bool> SendVerificationEmail(ApplicationUser user)
    {
        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
        var host = _httpContextAccessor.HttpContext.Request.Host.Value;
        var verificationUrl = $"{scheme}://{host}/api/auth/email/verification"
                              + $"?userId={user.Id}&token={emailConfirmationToken}";
        
        var subject = MailContentConstants.Subject;
        var text = MailContentConstants.GetBody(verificationUrl);
        
        return await _emailService.SendEmail(user.Email,
            subject,
            text);
    }
}