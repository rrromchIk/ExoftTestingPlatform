using TestingApi.Dto.UserDto;
using TestingApi.Helpers;

namespace TestingApi.Services.Abstractions;

public interface IUserService
{
    Task<PagedList<UserResponseDto>> GetAllUsersAsync(UserFiltersDto userFiltersDto,
        CancellationToken cancellationToken = default);
    Task<UserResponseDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserResponseDto> CreateUserAsync(UserDto userDto, CancellationToken cancellationToken = default);
    Task UpdateUserAsync(Guid id, UserDto userDto, CancellationToken cancellationToken = default);
    Task UpdateUserAvatarAsync(Guid id, string profilePictureFilePath, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
}