using TestingApi.Dto.TestDto;
using TestingApi.Models;

namespace TestingApi.Dto.UserTestDto;

public class StartedTestResponseDto
{
    public TestResponseDto Test { get; set; } = null!;
    public float Result { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime EndingTime { get; set; }
    public string UserTestStatus { get; set; } = null!;
}