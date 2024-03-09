using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Security.Dto;
using Security.Exceptions;
using Security.Models;
using Security.Service.Abstractions;

namespace Security.Service.Implementations;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenGenerator _tokenGenerator;

    public UserService(IMapper mapper, UserManager<ApplicationUser> userManager, ILogger<UserService> logger, ITokenGenerator tokenGenerator)
    {
        _mapper = mapper;
        _userManager = userManager;
        _logger = logger;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<UserResponseDto> CreateUserAsync(UserSignUpDto userSignUpDto)
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

        return _mapper.Map<UserResponseDto>(userSignUpDto);
    }

    public async Task<TokenResponseDto> Login(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

        if (user == null)
            throw new AuthException("User with such email not found", StatusCodes.Status404NotFound);
        
        var validPassword = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        if (!validPassword)
            throw new AuthException("User unauthorized", StatusCodes.Status401Unauthorized);
        
        
        user.RefreshToken = _tokenGenerator.GenerateRefreshToken(user);
        await _userManager.UpdateAsync(user);
        
        return new TokenResponseDto
        {
            AccessToken = _tokenGenerator.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken
        };
    }
}