namespace TestingApi.Dto.UserDto;

public class UserFiltersDto : FiltersDto
{
    public string? Role { get; set; }
    public bool? EmailConfirmed { get; set; }
}