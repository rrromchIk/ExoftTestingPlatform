using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApi.Constants;
using TestingApi.Data;
using TestingApi.Dto.UserDto;
using TestingAPI.Exceptions;
using TestingApi.Helpers;
using TestingApi.Services.Abstractions;
using Microsoft.Extensions.Options;
using TestingApi.Models.User;


namespace TestingApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly HttpClient _httpClient;
    private readonly SecurityHttpClientConstants _securityHttpClientConstants;
    private readonly ICurrentUserService _currentUserService;

    public UserService(DataContext dataContext, ILogger<UserService> logger,
        IMapper mapper, IHttpClientFactory httpClientFactory,
        IOptions<SecurityHttpClientConstants> securityHttpClientConstants, ICurrentUserService currentUserService)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _securityHttpClientConstants = securityHttpClientConstants.Value;
        _httpClient = httpClientFactory.CreateClient(_securityHttpClientConstants.ClientName);
        _logger = logger;
    }

    public async Task<PagedList<UserResponseDto>> GetAllUsersAsync(UserFiltersDto filtersDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> usersQuery = _dataContext.Users;

        usersQuery = ApplyFilters(usersQuery, filtersDto);

        usersQuery = filtersDto.SortOrder?.ToLower() == "asc"
            ? usersQuery.OrderBy(GetSortProperty(filtersDto.SortColumn, "asc"))
            : usersQuery.OrderByDescending(GetSortProperty(filtersDto.SortColumn, "desc"));

        var tests = await PagedList<User>.CreateAsync(
            usersQuery,
            filtersDto.Page,
            filtersDto.PageSize,
            cancellationToken
        );

        return _mapper.Map<PagedList<UserResponseDto>>(tests);
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

    public async Task<UserResponseDto> RegisterUserAsync(UserSignUpDto userSignUpDto,
        CancellationToken cancellationToken = default, bool isAdmin = false)
    {
        var jsonContent = JsonSerializer.Serialize(userSignUpDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var url = _httpClient.BaseAddress.ToString();
        if (isAdmin)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                _currentUserService.AccessTokenRaw);
            url += _securityHttpClientConstants.RegisterAdminEndpoint;
        } 
        else
        {
            url += _securityHttpClientConstants.RegisterEndpoint;
        }
        
        var httpResponse = await _httpClient.PostAsync(url, content, cancellationToken);
        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            var problemDetails = await HandleHttpResponse<ProblemDetails>(httpResponse, cancellationToken);
            throw new ApiException(problemDetails.Detail, (int)httpResponse.StatusCode);
        }
        
        var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogInformation("Response body: {r}", responseBody);

        var securityUserResponseDto = await HandleHttpResponse<SecurityUserResponseDto>(httpResponse, cancellationToken);
        _logger.LogInformation("Security response: {r}", JsonSerializer.Serialize(securityUserResponseDto));

        var userToAdd = _mapper.Map<User>(securityUserResponseDto);
        var createdUser = await _dataContext.AddAsync(userToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponseDto>(createdUser.Entity);
    }

    public async Task ConfirmEmail(Guid userId, string token, CancellationToken cancellationToken = default)
    {
        var url = _httpClient.BaseAddress + _securityHttpClientConstants.ConfirmEmailEndpoint +
                  $"?userId={userId}&token={token}";
        var httpResponse = await _httpClient.GetAsync(url, cancellationToken);
        
        _logger.LogInformation("{b}, {s}",
            await httpResponse.Content.ReadAsStringAsync(cancellationToken),
            httpResponse.StatusCode);
        
        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            var problemDetails = await HandleHttpResponse<ProblemDetails>(httpResponse, cancellationToken);
            throw new ApiException(problemDetails.Detail, (int)httpResponse.StatusCode);
        }


        var userToConfirmEmail = await _dataContext.Users
            .FirstAsync(u => u.Id == userId, cancellationToken);

        userToConfirmEmail.EmailConfirmed = true;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
    {
        if (!await CheckRoleViolationsForUpdate(id, cancellationToken))
            throw new ApiException("Forbidden to update the user", StatusCodes.Status403Forbidden);
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            _currentUserService.AccessTokenRaw);
        
        var jsonContent = JsonSerializer.Serialize(userUpdateDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, ContentType.ApplicationJson.ToString());

        var url = _httpClient.BaseAddress + _securityHttpClientConstants.UpdateEndpoint + $"/{id}";
        
        var httpResponse = await _httpClient.PatchAsync(url, content, cancellationToken);
        
        if (httpResponse.StatusCode != HttpStatusCode.NoContent)
            throw new ApiException("Error while updating user", (int)httpResponse.StatusCode);
        
        var userFounded = await _dataContext.Users.FirstAsync(u => u.Id == id, cancellationToken);
        userFounded.FirstName = userUpdateDto.FirstName;
        userFounded.LastName = userUpdateDto.LastName;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAvatarAsync(Guid id, string profilePictureFilePath,
        CancellationToken cancellationToken = default)
    {
        if(!await CheckRoleViolationsForUpdate(id, cancellationToken))
            throw new ApiException("Forbidden to update the user", StatusCodes.Status403Forbidden);
        
        var userToUpdate = await _dataContext.Users.FirstAsync(u => u.Id == id, cancellationToken);

        userToUpdate.ProfilePictureFilePath = profilePictureFilePath;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await CheckRoleViolationsForDelete(id, cancellationToken))
            throw new ApiException("Forbidden to delete user", StatusCodes.Status403Forbidden);
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            _currentUserService.AccessTokenRaw);
        var url = _httpClient.BaseAddress + _securityHttpClientConstants.DeleteEndpoint + $"/{id}";
        var httpResponse = await _httpClient.DeleteAsync(url, cancellationToken);

        _logger.LogInformation("Response: Status {s}, Body: {b}",
            httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
        
        if (httpResponse.StatusCode != HttpStatusCode.NoContent)
            throw new ApiException("Error while deleting user", (int)httpResponse.StatusCode);
        
        var userToDelete = await _dataContext.Users
            .FirstAsync(e => e.Id == id, cancellationToken);

        _dataContext.Remove(userToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }


    private static async Task<T> HandleHttpResponse<T>(HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        var securityUserResponseDto = await JsonSerializer.DeserializeAsync<T>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            },
            cancellationToken
        );

        return securityUserResponseDto;
    }

    private async Task<bool> CheckRoleViolationsForUpdate(Guid userToUpdateId, CancellationToken cancellationToken = default)
    {
        var currentUserRole = _currentUserService.UserRole;
        var currentUserId = _currentUserService.UserId;
        var userToUpdateRole = (await _dataContext.Users
            .FirstAsync(u => u.Id == userToUpdateId, cancellationToken))
            .UserRole.ToString();
        
        switch (currentUserRole)
        {
            case "User" when userToUpdateRole != "User" ||
                             currentUserId != userToUpdateId.ToString():
            case "Admin" when userToUpdateRole == "SuperAdmin":
            case "Admin" when userToUpdateRole == "Admin" &&
                              currentUserId != userToUpdateId.ToString():
                return false;
            default:
                return true;
        }
    }
    
    private async Task<bool> CheckRoleViolationsForDelete(Guid userToDeleteId, CancellationToken cancellationToken = default)
    {
        var userToDeleteRole = (await _dataContext.Users
                .FirstAsync(u => u.Id == userToDeleteId, cancellationToken))
            .UserRole.ToString();

        switch (userToDeleteRole)
        {
            case "SuperAdmin":
            case "Admin" when _currentUserService.UserRole != "SuperAdmin":
                return false;
            default:
                return true;
        }
    }
    
    private IQueryable<User> ApplyFilters(IQueryable<User> query, UserFiltersDto userFiltersDto) {
        if (userFiltersDto.EmailConfirmed != null) {
            query = query
                .Where(u => u.EmailConfirmed == userFiltersDto.EmailConfirmed);
        }
        
        if (!string.IsNullOrEmpty(userFiltersDto.Role)) {
            if (Enum.TryParse(typeof(UserRole), userFiltersDto.Role, true, out var roleValue)) {
                query = query.Where(u => u.UserRole == (UserRole)roleValue);
            }
        }
        
        if (!string.IsNullOrWhiteSpace(userFiltersDto.SearchTerm))
        {
            query = query.Where(
                u =>
                    u.FirstName.Contains(userFiltersDto.SearchTerm) ||
                    u.LastName.Contains(userFiltersDto.SearchTerm) ||
                    u.Email.Contains(userFiltersDto.SearchTerm)
            );
        }

        return query;
    }

    private static Expression<Func<User, object>> GetSortProperty(string? sortColumn, string sortOrder)
    {
        return sortColumn?.ToLower() switch
        {
            "modificationdate" => u => u.ModifiedTimestamp == null 
                ? sortOrder == "asc" 
                    ? DateTime.MaxValue
                    : DateTime.MinValue
                : u.ModifiedTimestamp,
            "creationdate" => u => u.CreatedTimestamp,
            _ => u => u.CreatedTimestamp
        };
    }
}