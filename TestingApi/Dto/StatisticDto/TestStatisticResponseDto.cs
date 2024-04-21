using TestingApi.Dto.TestDto;

namespace TestingApi.Dto.StatisticDto;

public class TestStatisticResponseDto
{
    public TestResponseDto Test { get; set; } = null!;
    public int TotalAmountOfAttemptsTaken { get; set; }
    public int AmountOfCurrentGoingAttempts { get; set; }
    public float AverageUsersTimeSpentInMinutes { get; set; }
    public float AverageUsersResult { get; set; }
    public ICollection<float> AllUsersResults { get; set; } = null!;
}