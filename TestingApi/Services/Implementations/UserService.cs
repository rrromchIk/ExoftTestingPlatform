using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.UserDto;
using TestingApi.Helpers;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TestService> _logger;

    public UserService(DataContext dataContext, ILogger<TestService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<UserResponseDto>> GetAllUsersAsync(UserFiltersDto userFiltersDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> usersQuery = _dataContext.Users;

        if (!string.IsNullOrWhiteSpace(userFiltersDto.SearchTerm))
        {
            usersQuery = usersQuery.Where(
                u =>
                    u.Name.Contains(userFiltersDto.SearchTerm) ||
                    u.Surname.Contains(userFiltersDto.SearchTerm) ||
                    u.Email.Contains(userFiltersDto.SearchTerm)
            );
        }

        usersQuery = userFiltersDto.SortOrder?.ToLower() == "desc"
            ? usersQuery.OrderByDescending(GetSortProperty(userFiltersDto.SortColumn))
            : usersQuery.OrderBy(GetSortProperty(userFiltersDto.SortColumn));

        var tests = await PagedList<User>.CreateAsync(
            usersQuery,
            userFiltersDto.Page,
            userFiltersDto.PageSize,
            cancellationToken
        );

        return _mapper.Map<PagedList<UserResponseDto>>(tests);
    }
    
    private static Expression<Func<User, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => u => u.Name,
            "surname" => u => u.Surname,
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

        var createdUser = await _dataContext.AddAsync(userToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponseDto>(createdUser.Entity);
    }

    public async Task UpdateUserAsync(Guid id, UserDto userDto, CancellationToken cancellationToken = default)
    {
        var userFounded = await _dataContext.Users.FirstAsync(u => u.Id == id, cancellationToken);
        var updatedUser = _mapper.Map<User>(userDto);
        
        userFounded.Name = updatedUser.Name;
        userFounded.Surname = updatedUser.Surname;
        userFounded.UserRole = updatedUser.UserRole;

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