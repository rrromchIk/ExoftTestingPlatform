using Security.Dto;

namespace Security.Service.Abstractions;

public interface IUserService
{
    Task DeleteUser(Guid userId);
    Task UpdateUser(Guid userId, UserUpdateDto userUpdateDto);
}