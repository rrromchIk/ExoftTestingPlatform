using TestingApi.Dto.TestDto;

namespace TestingApi.Dto.UserTestDto;

public class UserTestResponseDto
{
    public Guid UserId { get; set; }
    public TestResponseDto Test { get; set; } = null!;
    public float TotalScore { get; set; }
    public float UserScore { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime EndingTime { get; set; }
    public string UserTestStatus { get; set; } = null!;
}