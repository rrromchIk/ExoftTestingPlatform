namespace TestingApi.Services.Abstractions;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    string? UserRole { get; }
}