using TestingApi.Models;

namespace TestingApi.Dto.UserTestDto;

public class UserTestResponseDto
{
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }

    public float Result { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime EndingTime { get; set; }
    public string UserTestStatus { get; set; } = null!;
}