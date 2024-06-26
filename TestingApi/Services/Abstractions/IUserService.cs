﻿using TestingApi.Dto.UserDto;
using TestingApi.Helpers;

namespace TestingApi.Services.Abstractions;

public interface IUserService
{
    Task<PagedList<UserResponseDto>> GetAllUsersAsync(UserFiltersDto filtersDto,
        CancellationToken cancellationToken = default);
    Task<UserResponseDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserResponseDto> RegisterUserAsync(UserSignUpDto userSignUpDto, CancellationToken cancellationToken = default,
        bool isAdmin = false);
    Task ConfirmEmail(EmailConfirmationDto emailConfirmationDto, CancellationToken cancellationToken = default);
    Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default);
    Task UpdateUserAvatarAsync(Guid id, string profilePictureFilePath, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
}