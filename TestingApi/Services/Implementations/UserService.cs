using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto;
using TestingApi.Dto.UserDto;
using TestingAPI.Exceptions;
using TestingApi.Helpers;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(DataContext dataContext, ILogger<UserService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<UserResponseDto>> GetAllUsersAsync(FiltersDto filtersDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> usersQuery = _dataContext.Users;

        if (!string.IsNullOrWhiteSpace(filtersDto.SearchTerm))
        {
            usersQuery = usersQuery.Where(
                u =>
                    u.FirstName.Contains(filtersDto.SearchTerm) ||
                    u.LastName.Contains(filtersDto.SearchTerm) ||
                    u.Email.Contains(filtersDto.SearchTerm)
            );
        }

        usersQuery = filtersDto.SortOrder?.ToLower() == "desc"
            ? usersQuery.OrderByDescending(GetSortProperty(filtersDto.SortColumn))
            : usersQuery.OrderBy(GetSortProperty(filtersDto.SortColumn));

        var tests = await PagedList<User>.CreateAsync(
            usersQuery,
            filtersDto.Page,
            filtersDto.PageSize,
            cancellationToken
        );

        return _mapper.Map<PagedList<UserResponseDto>>(tests);
    }
    
    private static Expression<Func<User, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => u => u.FirstName,
            "surname" => u => u.LastName,
            "email" => u => u.Email,
            "creationTime" => u => u.CreatedTimestamp,
            _ => u => u.Id
        };
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Users
            .AnyAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<UserResponseDto> CreateUserAsync(UserDto userDto, CancellationToken cancellationToken = default)
    {
        var userToAdd = _mapper.Map<User>(userDto);

        var collision = await _dataContext.Users
            .AnyAsync(
                u => u.Email == userToAdd.Email,
                cancellationToken
            );

        if (collision)
            throw new ApiException("User email has to be unique", StatusCodes.Status409Conflict);
        var createdUser = await _dataContext.AddAsync(userToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponseDto>(createdUser.Entity);
    }

    public async Task UpdateUserAsync(Guid id, UserDto userDto, CancellationToken cancellationToken = default)
    {
        var userFounded = await _dataContext.Users.FirstAsync(u => u.Id == id, cancellationToken);
        var updatedUser = _mapper.Map<User>(userDto);
        
        userFounded.FirstName = updatedUser.FirstName;
        userFounded.LastName = updatedUser.LastName;
        userFounded.UserRole = updatedUser.UserRole;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAvatarAsync(Guid id, string profilePictureFilePath, CancellationToken cancellationToken = default)
    {
        var userToUpdate = await _dataContext.Users.FirstAsync(u => u.Id == id, cancellationToken);

        userToUpdate.ProfilePictureFilePath = profilePictureFilePath;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userToDelete = await _dataContext.Users
            .FirstAsync(e => e.Id == id, cancellationToken);

        _dataContext.Remove(userToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}