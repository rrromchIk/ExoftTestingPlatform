using Microsoft.AspNetCore.Identity;
using Security.Dto;
using Security.Exceptions;
using Security.Models;
using Security.Service.Abstractions;

namespace Security.Service.Implementations;

public class UserService : IUserService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _currentUserService = currentUserService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task DeleteUser(Guid userId)
    {
        var userToDelete = await _userManager.FindByIdAsync(userId.ToString());
        if (userToDelete == null)
            throw new AuthException("Usr with such id not found", StatusCodes.Status404NotFound);

        var userToDeleteRoles = await _userManager.GetRolesAsync(userToDelete);

        if (userToDeleteRoles.Contains("SuperAdmin"))
            throw new AuthException("Can not delete SuperAdmin", StatusCodes.Status400BadRequest);

        if (userToDeleteRoles.Contains("Admin") && _currentUserService.UserRole != "SuperAdmin")
            throw new AuthException(
                _currentUserService.UserRole + " can not delete Admin",
                StatusCodes.Status403Forbidden
            );

        var result = await _userManager.DeleteAsync(userToDelete);
        if (!result.Succeeded)
            throw new AuthException(result.Errors.First().Description, StatusCodes.Status500InternalServerError);
    }

    public async Task UpdateUser(Guid userId, UserUpdateDto userUpdateDto)
    {
        var userToUpdate = await _userManager.FindByIdAsync(userId.ToString());
        if (userToUpdate == null)
            throw new AuthException("Usr with such id not found", StatusCodes.Status404NotFound);

        if(!await CheckUpdateViolation(userToUpdate))
            throw new AuthException("Forbidden to update the user", StatusCodes.Status403Forbidden);
        
        userToUpdate.FirstName = userToUpdate.FirstName;
        userToUpdate.LastName = userToUpdate.LastName;
        var result = await _userManager.UpdateAsync(userToUpdate);
        if (!result.Succeeded)
            throw new AuthException(result.Errors.First().Description, StatusCodes.Status500InternalServerError);
    }

    private async Task<bool> CheckUpdateViolation(ApplicationUser userToUpdate)
    {
        var userToUpdateRoles = await _userManager.GetRolesAsync(userToUpdate);
        
        switch (_currentUserService.UserRole)
        {
            case "User" when !userToUpdateRoles.Contains("User") ||
                             _currentUserService.UserId != userToUpdate.Id.ToString():
            case "Admin" when userToUpdateRoles.Contains("SuperAdmin"):
            case "Admin" when userToUpdateRoles.Contains("Admin") &&
                              _currentUserService.UserId != userToUpdate.Id.ToString():
                return false;
            default:
                return true;
        }
    }
}