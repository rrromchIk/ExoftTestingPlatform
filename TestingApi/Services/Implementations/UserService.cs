using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApi.Constants;
using TestingApi.Data;
using TestingApi.Dto;
using TestingApi.Dto.UserDto;
using TestingAPI.Exceptions;
using TestingApi.Helpers;
using TestingApi.Models;
using TestingApi.Services.Abstractions;
using Microsoft.Extensions.Options;


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

    public async Task UpdateUserAsync(Guid id, UserDto userDto, CancellationToken cancellationToken = default)
    {
        var userFounded = await _dataContext.Users.FirstAsync(u => u.Id == id, cancellationToken);
        var updatedUser = _mapper.Map<User>(userDto);

        userFounded.FirstName = updatedUser.FirstName;
        userFounded.LastName = updatedUser.LastName;
        userFounded.UserRole = updatedUser.UserRole;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAvatarAsync(Guid id, string profilePictureFilePath,
        CancellationToken cancellationToken = default)
    {
        var userToUpdate = await _dataContext.Users.FirstAsync(u => u.Id == id, cancellationToken);

        userToUpdate.ProfilePictureFilePath = profilePictureFilePath;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Access token: {a}", _currentUserService.AccessTokenRaw);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            _currentUserService.AccessTokenRaw);
        var httpResponse = await _httpClient.DeleteAsync(
            _httpClient.BaseAddress + _securityHttpClientConstants.UpdateEndpoint + $"/{id}",
            cancellationToken
        );

        _logger.LogInformation("Response: Status {s}, Body: {b}",
            httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
        
        if (httpResponse.StatusCode != HttpStatusCode.NoContent)
        {
            throw new ApiException("Error while deleting user", (int)httpResponse.StatusCode);
        }
        
        var userToDelete = await _dataContext.Users
            .FirstAsync(e => e.Id == id, cancellationToken);

        _dataContext.Remove(userToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }


    private static async Task<T> HandleHttpResponse<T>(HttpResponseMessage httpResponse, CancellationToken cancellationToken)
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
}